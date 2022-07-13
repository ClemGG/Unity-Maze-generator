using System.Collections.Generic;

namespace Project.Procedural.MazeGeneration
{

    /* The Binary Tree Algorithm takes the northern and eastern neighbors of each Cell,
     * and if any, links them together at random.
     * This creates a Bias in the Maze where its northern and eastrn sides will be giant passages
     * with no walls.
     */
    public static class BinaryTree
    {
        private static List<Cell> _neighbors { get; set; }

        public static void Execute(Grid grid)
        {
            foreach (Cell cell in grid.EachCell())
            {
                _neighbors = new();

                if (cell.North != null) _neighbors.Add(cell.North);
                if (cell.East != null) _neighbors.Add(cell.East);

                Cell neighbor = _neighbors.Sample();

                if (neighbor != null) cell.Link(neighbor);
            }
        }
    }
}