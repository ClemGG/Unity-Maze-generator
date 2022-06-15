using System;
using System.Collections.Generic;

namespace Project.Procedural.MazeGeneration
{
    public class Cell
    {
        public int Row { get; }
        public int Column { get; }

        public Cell North { get; set; }
        public Cell South { get; set; }
        public Cell East { get; set; }
        public Cell West { get; set; }

        //Cells joined by a passage to this Cell
        public Dictionary<Cell, bool> Links { get; set; }

        private List<Cell> _neighbors;
        public List<Cell> Neighbors
        {
            get
            {
                _neighbors.Clear();
                if (North is not null) _neighbors.Add(North);
                if (South is not null) _neighbors.Add(South);
                if (East is not null) _neighbors.Add(East);
                if (West is not null) _neighbors.Add(West);
                return _neighbors;
            }
        }



        public Cell(int row, int column)
        {
            Row = row;
            Column = column;
            Links = new();
        }

        //bidi makes sure the operation occurs bidirectionally, so that the connection
        //is recorded on both cells
        public void Link(Cell cell, bool bidi = true)
        {
            if (!Links.ContainsKey(cell))
            {
                Links.Add(cell, true);
            }
            if (bidi)
            {
                cell.Link(this, false);
            }
        }

        public void Unlink(Cell cell, bool bidi = true)
        {
            if (Links.ContainsKey(cell))
            {
                Links.Remove(cell);
            }
            if (bidi)
            {
                cell.Unlink(this, false);
            }
        }

        public IEnumerable<Cell> GetAllLinkedCells()
        {
            foreach (Cell item in Links.Keys)
            {
                yield return item;
            }
        }

        public bool IsLinked(Cell cell)
        {
            if(cell is null)
            {
                return false;
            }
            return Links.ContainsKey(cell);
        }
    }
}