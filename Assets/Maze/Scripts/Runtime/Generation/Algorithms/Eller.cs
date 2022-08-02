using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{

    //Combines the Sidewinder algorithm (using 1 row at a time) and the Kruskal algorithm
    //(which separates cells into sets with costs)
    public class Eller : IGeneration
    {
        public GenerationProgressReport Report { get; set; } = new();

        private class RowState
        {
            public Dictionary<int, int> SetForCell { get; }
            public Dictionary<int, List<Cell>> CellsInSet { get; }
            public int NextSet { get; private set; }

            public RowState(int startingSet = 0)
            {
                SetForCell = new();
                CellsInSet = new();
                NextSet = startingSet;
            }

            public void Record(int set, Cell cell)
            {
                SetForCell.Add(cell.Column, set);

                if (!CellsInSet.ContainsKey(set))
                {
                    CellsInSet.Add(set, new());
                }
                CellsInSet[set].Add(cell);
            }

            public int SetFor(Cell cell)
            {
                if (!SetForCell.ContainsKey(cell.Column))
                {
                    Record(NextSet, cell);
                    NextSet++;
                }

                return SetForCell[cell.Column];
            }

            public void Merge(int winner, int loser)
            {
                foreach (Cell cell in CellsInSet[loser])
                {
                    SetForCell[cell.Column] = winner;
                    CellsInSet[winner].Add(cell);
                }

                CellsInSet.Remove(loser);
            }

            public RowState Next()
            {
                return new(NextSet);
            }

            public IEnumerable<KeyValuePair<int, List<Cell>>> EachSet()
            {
                foreach (var pair in CellsInSet)
                {
                    yield return pair;
                }
            }
        }



        public void ExecuteSync(IGrid grid, Cell start = null)
        {
            RowState rowState = new();

            foreach (Cell[] row in grid.EachRow())
            {
                foreach (Cell cell in row)
                {
                    if (cell.West is null) continue;

                    int set = rowState.SetFor(cell);
                    int priorSet = rowState.SetFor(cell.West);

                    bool shouldLink = set != priorSet && (cell.South is null || 2.Sample() == 0);

                    if (shouldLink)
                    {
                        cell.Link(cell.West);
                        rowState.Merge(set, priorSet);
                    }
                }


                if(row[0].South is not null)
                {
                    RowState nextRow = rowState.Next();

                    foreach (var pair in rowState.EachSet())
                    {
                        var list = pair.Value.Shuffle();
                        foreach (var cell in list)
                        {
                            int index = list.IndexOf(cell);
                            if (index == 0 || 3.Sample() == 0)
                            {
                                cell.Link(cell.South);
                                nextRow.Record(rowState.SetFor(cell), cell.South);
                            }
                        }
                    }

                    rowState = nextRow;
                }
            }


        }



        public IEnumerator ExecuteAsync(IGrid grid, IProgress<GenerationProgressReport> progress, Cell start = null)
        {
            
            List<Cell> linkedCells = new();

            RowState rowState = new();
            foreach (Cell[] row in grid.EachRow())
            {
                foreach (Cell cell in row)
                {
                    if (cell.West is null) continue;

                    int set = rowState.SetFor(cell);
                    int priorSet = rowState.SetFor(cell.West);

                    bool shouldLink = set != priorSet && (cell.South is null || 2.Sample() == 0);

                    if (shouldLink)
                    {
                        cell.Link(cell.West);
                        rowState.Merge(set, priorSet);

                        linkedCells.Add(cell);
                        Report.ProgressPercentage = (float)(linkedCells.Count * 100 / grid.Size()) / 100f;
                        Report.UpdateTrackTime(Time.deltaTime);
                        progress.Report(Report);
                        yield return null;
                    }
                }


                if (row[0].South is not null)
                {
                    RowState nextRow = rowState.Next();

                    foreach (var pair in rowState.EachSet())
                    {
                        var list = pair.Value.Shuffle();
                        foreach (var cell in list)
                        {
                            int index = list.IndexOf(cell);
                            if (index == 0 || 3.Sample() == 0)
                            {
                                cell.Link(cell.South);
                                nextRow.Record(rowState.SetFor(cell), cell.South);

                                linkedCells.Add(cell);
                                Report.ProgressPercentage = (float)(linkedCells.Count * 100 / grid.Size()) / 100f;
                                Report.UpdateTrackTime(Time.deltaTime);
                                progress.Report(Report);
                                yield return null;
                            }
                        }
                    }

                    rowState = nextRow;
                }
            }


            Report.ProgressPercentage = (float)(linkedCells.Count * 100 / grid.Size()) / 100f;
            Report.UpdateTrackTime(Time.deltaTime);
            progress.Report(Report);
            yield return null;
        }
    }
}