using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public abstract class Grid : IGrid
    {
        public int Rows { get; }
        public int Columns { get; }
        public float BraidRate { get; } = 0f;



        public Distances Distances { get; set; }
        public Cell Farthest { get; set; }
        public int Maximum { get; set; }

        public virtual int Size() => Rows * Columns;
        public virtual Cell RandomCell()
        {
            return _grid[Rows.Sample()][Columns.Sample()];
        }


        protected Cell[][] _grid;


        public Grid(int rows, int columns, float braidRate = 0f)
        {
            Rows = rows;
            Columns = columns;
            BraidRate = braidRate;

            PrepareGrid();
            ConfigureCells();
        }


        public Grid(GenerationSettingsSO generationSettings) : 
            this(generationSettings.GridSize.x, generationSettings.GridSize.y, generationSettings.BraidRate)
        {
        }

        public Cell this[int row, int column]
        {
            get
            {
                if(row >= 0 && row < Rows && column >= 0 && column < Columns)
                {
                    return _grid[row][column];
                }

                return null;
            }
        }

        protected virtual void PrepareGrid()
        {
            _grid = new Cell[Rows][];

            for (int i = 0; i < Rows; i++)
            {
                _grid[i] = new Cell[Columns];
                for (int j = 0; j < Columns; j++)
                {
                    _grid[i][j] = new(i, j);
                }
            }

        }

        protected virtual void ConfigureCells()
        {
            for(int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Cell c = _grid[i][j];
                    c.North = this[i - 1, j];
                    c.South = this[i + 1, j];
                    c.West = this[i, j - 1];
                    c.East = this[i, j + 1];
                }
            }
        }

        public IEnumerable<Cell[]> EachRow()
        {
            for (int i = 0; i < _grid.Length; i++)
            {
                yield return _grid[i];
            }
        }
        public IEnumerable<Cell> EachCell()
        {
            foreach (Cell[] row in EachRow())
            {
                for (int i = 0; i < row.Length; i++)
                {
                    if(row[i] is not null)
                        yield return row[i];
                }
            }
        }


        public List<Cell> GetDeadends()
        {
            List<Cell> deadends = new();
            foreach (Cell cell in EachCell())
            {
                if(cell.Links.Count == 1)
                {
                    deadends.Add(cell);
                }
            }

            return deadends;
        }

        //Removes dead ends from the maze to create a braided (imperfect) maze.
        //The higher the braidRate, the more deadends are removed.
        public void Braid()
        {
            List<Cell> deadends = GetDeadends();
            List<Cell> shuffledDeadends = deadends.Shuffle();

            //Because of rounding, this method will not always remove the exact number of deadends requested,
            //but it's still more accurate than the previous version with a percentage.
            int nbDeadendsToRemove = Mathf.RoundToInt(Mathf.Lerp(0f, (float)deadends.Count, BraidRate));


            for (int i = 0; i < nbDeadendsToRemove; i++)
            {
                Cell cell = shuffledDeadends[i];

                //Checks links.count to avoid repeating a cell
                if (cell.Links.Count != 1)
                    continue;

                //get unlinked neighbors
                List<Cell> neighbors = new(cell.Neighbors.Count);
                foreach (Cell neighbor in cell.Neighbors)
                {
                    if (!cell.IsLinked(neighbor))
                    {
                        neighbors.Add(neighbor);
                    }
                }

                //get the best neighbor
                //optimizes by prefering to link 2 deadends together
                //Commented for now, but we'll keep it just in case.
                List<Cell> best = new(neighbors.Count);
                //foreach (Cell n in neighbors)
                //{
                //    if (n.Links.Count == 1)
                //    {
                //        best.Add(n);
                //    }
                //}

                //We avoid linking 2 deadends together if possible
                best = neighbors.Where(cell => cell.Links.Count > 1).ToList();

                if (best.Count == 0)
                {
                    best = neighbors;
                }

                Cell bestNeighbor = best.Sample();
                cell.Link(bestNeighbor);
            }

        }

        //Links all cells together to create an empty maze
        public void LinkAll()
        {
            foreach (Cell cell in EachCell())
            {
                foreach (Cell n in cell.Neighbors)
                {
                    cell.Link(n, false);
                }
            }
        }
        //Uninks all cells to create walls around every cell
        public void UnlinkAll()
        {
            foreach (Cell cell in EachCell())
            {
                foreach (Cell n in cell.Neighbors)
                {
                    cell.Unlink(n, false);
                }
            }
        }
    }
}