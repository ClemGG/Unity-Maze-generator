using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    //Kruskal generates weights between passages and merge Cells with the lowest weight
    //on the grid in their own set.
    public class RandomizedKruskal : IGeneration
    {
        public GenerationProgressReport Report { get; set; } = new();

        private class State
        {
            public List<Cell[]> Neighbors { get; }
            public Dictionary<Cell, int> SetForCell { get; }
            public Dictionary<int, List<Cell>> CellsInSet { get; }
            public IGrid Grid { get; }

            public State(IGrid grid)
            {
                Grid = grid;
                Neighbors = new();
                SetForCell = new();
                CellsInSet = new();

                foreach (Cell cell in grid.EachCell())
                {
                    int set = SetForCell.Count;
                    SetForCell.Add(cell, set);
                    CellsInSet.Add(set, new() { cell });

                    if (cell.South is not null)
                    {
                        Neighbors.Add(new Cell[] { cell, cell.South });
                    }
                    if (cell.East is not null)
                    {
                        Neighbors.Add(new Cell[] { cell, cell.East });
                    }
                }
            }

            public bool CanMerge(Cell left, Cell right)
            {
                return SetForCell[left] != SetForCell[right];
            }

            public void Merge(Cell left, Cell right)
            {
                left.Link(right);
                int winner = SetForCell[left];
                int loser = SetForCell[right];
                List<Cell> losers = CellsInSet[loser];
                if(losers.Count == 0)
                {
                    losers.Add(right);
                }

                for (int i = 0; i < losers.Count; i++)
                {
                    CellsInSet[winner].Add(losers[i]);
                    SetForCell[losers[i]] = winner;
                }

                CellsInSet.Remove(loser);
            }

            
        }


        public void ExecuteSync(IGrid grid, Cell start = null)
        {
            State state = new(grid);

            List<Cell[]> shuffledNeighbors = state.Neighbors.Shuffle();
            while (shuffledNeighbors.Count > 0)
            {
                Cell[] pair = shuffledNeighbors[shuffledNeighbors.Count - 1];
                shuffledNeighbors.Remove(pair);

                if (state.CanMerge(pair[0], pair[1]))
                {
                    state.Merge(pair[0], pair[1]);
                }
            }
        }



        public IEnumerator ExecuteAsync(IGrid grid, IProgress<GenerationProgressReport> progress, Cell start = null)
        {
            
            List<Cell> linkedCells = new();

            State state = new(grid);

            List<Cell[]> shuffledNeighbors = state.Neighbors.Shuffle();
            int count = shuffledNeighbors.Count;
            while (shuffledNeighbors.Count > 0)
            {
                Cell[] pair = shuffledNeighbors[shuffledNeighbors.Count - 1];
                shuffledNeighbors.Remove(pair);

                if (state.CanMerge(pair[0], pair[1]))
                {
                    state.Merge(pair[0], pair[1]);
                }
                Report.ProgressPercentage = (float)((count - shuffledNeighbors.Count) * 100 / grid.Size()) / 100f;
                Report.UpdateTrackTime(Time.deltaTime);
                progress.Report(Report);
                yield return null;
            }
        }
    }
}