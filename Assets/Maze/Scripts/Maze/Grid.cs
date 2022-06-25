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
        public virtual Cell RandomCell() => _grid[Rows.Sample()][Columns.Sample()];

        protected Cell[][] _grid;

        
        public Grid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;

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
    }
}