using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Procedural.MazeGeneration
{
    //The Growing Tree selects the active Cell by using a random lambda expression.
    //This allows us to mix different methods to replicate or combine the behaviour of other algorithms,
    //like the Simple Prim or the Recursive Backtracker.
    public static class GrowingTree
    {

        private static Func<List<Cell>, Cell> Lambda{ get; set; }

        /// <summary>
        /// This will let us choose how to select the Cells from the active list
        /// </summary>
        public static void SetLambdaMethod(Func<List<Cell>, Cell> lambdaToUse)
        {
            Lambda = lambdaToUse;
        }

        public static void Execute(Grid grid, Cell start = null)
        {
            if (start is null) start = grid.RandomCell();

            List<Cell> active = new() { start };

            while (active.Any())
            {
                Cell cell = Lambda.Invoke(active);
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