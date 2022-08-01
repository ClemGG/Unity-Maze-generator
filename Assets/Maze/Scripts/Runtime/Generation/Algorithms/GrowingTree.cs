using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Procedural.MazeGeneration
{
    //The Growing Tree selects the active Cell by using a random lambda expression.
    //This allows us to mix different methods to replicate or combine the behaviour of other algorithms,
    //like the Simple Prim or the Recursive Backtracker.
    public class GrowingTree : IGeneration
    {
        private Func<List<Cell>, Cell> Lambda { get; }


        public GrowingTree(GenerationSettingsSO generationSettings)
        {
            Lambda = SetLambda(generationSettings.LambdaSelection);
        }


        /// <summary>
        /// This will let us choose how to select the Cells from the active list
        /// </summary>
        private Func<List<Cell>, Cell> SetLambda(GrowingTreeLambda lambdaType) => lambdaType switch
        {
            //Selects a cell at random (executes Simple Prim)
            GrowingTreeLambda.Random => (active) => active.Sample(),
            //Selects the last cell (executes Recursive Backtracker)
            GrowingTreeLambda.LastCell => (active) => active.Last(),
            //Selects the first cell (creates elongated corridors)
            GrowingTreeLambda.FirstCell => (active) => active.First(),
            //Mixes between the Recursive Backtracker and the Simple Prim
            GrowingTreeLambda.FirstAndLastMix => (active) => (2.Sample() == 0) ? active.First() : active.Last(),
            _ => null,
        };



        public void ExecuteSync(IGrid grid, Cell start = null)
        {
            start ??= grid.RandomCell();

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