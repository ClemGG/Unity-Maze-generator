using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class Grid
    {
        public int Rows { get; }
        public int Columns { get; }

        public virtual int Size() => Rows * Columns;
        public virtual Cell RandomCell()
        {
            Cell cell;
            do
            {
                cell = _grid[Rows.Sample()][Columns.Sample()];
            } 
            while (cell is null);

            return cell;
        }


        protected Cell[][] _grid;


        public Grid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;

            PrepareGrid();
            ConfigureCells();
        }


        public Grid(GenerationSettingsSO generationSettings)
        {
            Rows = generationSettings.GridSize.x;
            Columns = generationSettings.GridSize.y;

            PrepareGrid();
            ConfigureCells();
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

        //Describes what to use to represent the Cell in the display classes
        protected virtual string ContentsOf(Cell cell)
        {
            return " ";
        }

        public virtual Color BackgroundColorFor(Cell cell)
        {
            return new(1, 1, 1, 1);
        }

        public override string ToString()
        {
            //6 refers to the numbers of colums per cell
            //3 refers to the numbers of rows per cell
            StringBuilder output = new(6*3*Rows*Columns);
            output.Append("+");
            output.Insert(1, "---+", Columns);
            output.Append("\n");

            foreach (Cell[] row in EachRow())
            {
                var top = "|";
                var bottom = "+";

                for (int i = 0; i < row.Length; i++)
                {
                    Cell cell = row[i];
                    if(cell is null)
                    {
                        cell = new(-1, -1);
                    }

                    var body = $" {ContentsOf(cell)} ";   //3 spaces for the Cell's body
                    var eastBoundary = cell.IsLinked(cell.East) ? " " : "|";
                    top = string.Concat(top, body, eastBoundary);

                    //3 spaces below, too
                    var southBoundary = cell.IsLinked(cell.South) ? "   " : "---";
                    var corner = "+";
                    bottom = string.Concat(bottom, southBoundary, corner);

                }
                output.Append($"{top}\n");
                output.Append($"{bottom}\n");
            }

            return output.ToString();
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
        public void Braid(float braidRate = 1f)
        {
            List<Cell> deadends = GetDeadends();
            //Shuffles the list of dead ends
            List<Cell> shuffledDeadends = new(deadends.Count);
            while (deadends.Count > 0)
            {
                int rand = deadends.Count.Sample();
                Cell deadend = deadends[rand];
                deadends.Remove(deadend);
                shuffledDeadends.Add(deadend);
            }

            foreach (Cell cell in shuffledDeadends)
            {
                float braidRand = 1f.Sample();

                //Checks links.count to avoid repeating a cell
                if (cell.Links.Count != 1 || braidRand >= braidRate)
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
                List<Cell> best = new(neighbors.Count);
                foreach (Cell n in neighbors)
                {
                    if (n.Links.Count == 1)
                    {
                        best.Add(n);
                    }
                }

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