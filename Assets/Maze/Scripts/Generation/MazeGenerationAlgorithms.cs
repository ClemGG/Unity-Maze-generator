namespace Project.Procedural.MazeGeneration
{
    public static class MazeGenerationAlgorithms
    {
        public static void Execute(this Grid grid, GenerationType algorithm, Cell start = null)
        {
            switch (algorithm)
            {
                case GenerationType.BinaryTree:
                    BinaryTree.Execute(grid);
                    break;
                case GenerationType.Sidewinder:
                    Sidewinder.Execute(grid);
                    break;
                case GenerationType.AldousBroder:
                    AldousBroder.Execute(grid);
                    break;
                case GenerationType.Wilson:
                    Wilson.Execute(grid);
                    break;
                case GenerationType.HuntAndKill:
                    HuntAndKill.Execute(grid);
                    break;
                case GenerationType.RecursiveBacktracker:
                    RecursiveBacktracker.Execute(grid, start);
                    break;
                case GenerationType.RandomizedKruskal:
                    RandomizedKruskal.Execute(grid);
                    break;
                case GenerationType.SimplifiedPrim:
                    SimplifiedPrim.Execute(grid, start);
                    break;
                case GenerationType.TruePrim:
                    TruePrim.Execute(grid, start);
                    break;
                case GenerationType.GrowingTree:
                    GrowingTree.Execute(grid, start);
                    break;
                case GenerationType.Eller:
                    Eller.Execute(grid);
                    break;
                case GenerationType.RecursiveDivision:
                    RecursiveDivision.Execute(grid);
                    break;
            }
        }
    }
}