using System.Linq;
using System.Collections.Generic;

namespace Project.Procedural.MazeGeneration
{
    //The Simplified Prim gives a cost to each passage between passages.
    //Unlike the True Prim, the Simplified gives all cells the same cost and chooses neightbors at random.
    public class SimplifiedPrim : IGeneration
    {
        public void Execute(IGrid grid, Cell start = null)
        {
            start ??= grid.RandomCell();

            List<Cell> active = new() { start };

            while(active.Any()){
                Cell cell = active.Sample();
                Cell[] availableNeighbors = cell.Neighbors.Where(n => n.Links.Count == 0).ToArray();

                if (availableNeighbors.Any())
                {
                    Cell neighbor = availableNeighbors.Sample();
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