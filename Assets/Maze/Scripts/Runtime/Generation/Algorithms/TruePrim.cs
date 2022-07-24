using System.Linq;
using System.Collections.Generic;

namespace Project.Procedural.MazeGeneration
{
    //The Simplified Prim gives a different cost to each passage between passages,
    //then selects the cells with the lowest cost.
    public class TruePrim : IGeneration
    {
        public void Execute(IGrid grid, Cell start = null)
        {
            start ??= grid.RandomCell();

            List<Cell> active = new() { start };
            Dictionary<Cell, int> costs = new();

            foreach (Cell cell in grid.EachCell())
            {
                costs.Add(cell, 100.Sample());
            }


            while (active.Any())
            {
                //Get Cell with minimum cost
                Cell cell = null;
                int minCost = 100;

                foreach (Cell c in active)
                {
                    if (costs[c] < minCost)
                    {
                        minCost = costs[c];
                        cell = c;
                    }
                }

                Cell[] availableNeighbors = cell.Neighbors.Where(n => n.Links.Count == 0).ToArray();

                if (availableNeighbors.Any())
                {
                    Cell neighbor = null;
                    int minNCost = 100;
                    foreach (Cell c in availableNeighbors)
                    {
                        if (costs[c] < minNCost)
                        {
                            minNCost = costs[c];
                            neighbor = c;
                        }
                    }

                    cell.Link(neighbor);
                    active.Add(neighbor);
                }
                else
                {
                    active.Remove(cell);
                }
            }
        }
    }
}