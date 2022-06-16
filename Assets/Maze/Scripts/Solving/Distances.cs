using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class Distances
    {
        public Cell Root { get; private set; }
        public Dictionary<Cell, int> Cells { get; private set; }

        public Distances(Cell root)
        {
            Root = root;
            Cells = new Dictionary<Cell, int>
            {
                { root, 0 }
            };
        }

        public int this[Cell cell]
        {
            get
            {
                if (Cells.ContainsKey(cell))
                {
                    return Cells[cell];
                }

                return -1;
            }
            set
            {
                Cells.Add(cell, value);
            }
        }

        public IEnumerable<Cell> GetAllCells()
        {
            foreach (Cell item in Cells.Keys)
            {
                yield return item;
            }
        }

        public Distances PathTo(Cell goal)
        {
            Cell current = goal;
            var breadcrumbs = new Distances(Root);
            breadcrumbs[current] = Cells[current];

            while(current != Root)
            {
                foreach (Cell neighbor in current.GetAllLinkedCells())
                {
                    //Retrives a cell with a distance number 1 unit lower
                    //than the current cell.
                    //This goes automatically from the goal to the start.
                    if(Cells[neighbor] < Cells[current])
                    {
                        breadcrumbs[neighbor] = Cells[neighbor];
                        current = neighbor;
                        break;
                    }
                }
            }



            return breadcrumbs;
        }
    }
}