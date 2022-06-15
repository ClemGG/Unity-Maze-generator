namespace Project.Procedural.MazeGeneration
{

    public enum GenerationType : byte
    {
        BinaryTree,
        Sidewinder,
    }


    public static class MazeGenerationAlgorithms
    {
        public static void Execute(this Grid grid, GenerationType algorithm)
        {
            switch (algorithm)
            {
                case GenerationType.BinaryTree:
                    BinaryTree.Execute(grid);
                    break;
                case GenerationType.Sidewinder:
                    Sidewinder.Execute(grid);
                    break;
            }
        }
    }
}