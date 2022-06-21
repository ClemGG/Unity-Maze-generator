using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    //The Hunt-And-Kill algorithm functions similarily to the A-B and Wilson's algorithms,
    //excepts when the current cell is in a corner with no unvisited Cells,
    //the algo goes into Hunt mode, where it will scan each row from the top,
    //find the first unvisited cell next to a visited one
    //and restart the process from that point after linking it
    //to a previously visited neighboring cell.
    public static class HuntAndKill
    {
        public static void Execute(Grid grid)
        {
            Cell current = grid.RandomCell;

            while(current is not null)
            {
                List<Cell> unvisitedCells = new();
                for (int i = 0; i < current.Neighbors.Count; i++)
                {
                    Cell unvNeighbor = current.Neighbors[i];
                    if(unvNeighbor.Links.Count == 0)
                    {
                        unvisitedCells.Add(unvNeighbor);
                    }
                }

                if(unvisitedCells.Count > 0)
                {
                    Cell neighbor = unvisitedCells.Sample();
                    current.Link(neighbor);
                    current = neighbor;
                }
                else
                {
                    current = null;
                }

                foreach (Cell cell in grid.EachCell())
                {

                    List<Cell> visitedCells = new();
                    for (int i = 0; i < cell.Neighbors.Count; i++)
                    {
                        Cell vNeighbor = cell.Neighbors[i];
                        if (vNeighbor.Links.Count > 0)
                        {
                            unvisitedCells.Add(vNeighbor);
                        }
                    }

                    if(cell.Links.Count == 0 && visitedCells.Count > 0)
                    {
                        current = cell;
                        Cell neighbor = visitedCells.Sample();
                        current.Link(neighbor);
                        break;
                    }
                }
            }
        }
    }
}