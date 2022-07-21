using System.Collections.Generic;

namespace Project.Procedural.MazeGeneration
{

    /* The Binary Tree Algorithm takes the northern and eastern neighbors of each Cell,
     * and if any, links them together at random.
     * This creates a Bias in the Maze where its northern and eastrn sides will be giant passages
     * with no walls.
     */
    public class BinaryTree : IGeneration
    {
        private List<Cell> Neighbors { get; } = new();

        public void Execute(Grid grid)
        {
            foreach (Cell cell in grid.EachCell())
            {
                Neighbors.Clear();

                if (cell.North != null) Neighbors.Add(cell.North);
                if (cell.East != null) Neighbors.Add(cell.East);

                Cell neighbor = Neighbors.Sample();

                if (neighbor != null) cell.Link(neighbor);
            }
        }
    }
}