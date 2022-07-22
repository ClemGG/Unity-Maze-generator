namespace Project.Procedural.MazeGeneration
{
    public class RandomizedKruskalDemo : MazeGenerator
    {
        public override void SetupGrid()
        {
            Grid = new ColoredGrid(Settings);
        }

        public override void Generate()
        {
            RandomizedKruskal algorithm = new();
            algorithm.Execute(Grid);

            Cell start = Grid[Grid.Rows / 2, Grid.Columns / 2];
            (Grid as ColoredGrid).SetDistances(start.GetDistances());
        }
    }
}