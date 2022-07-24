namespace Project.Procedural.MazeGeneration
{
    public class BraidedMazeDemo : MazeGenerator
    {
        public override void SetupGrid()
        {
            Grid = new ColoredGrid(Settings);
        }

        public override void Generate()
        {
            RecursiveBacktracker algorithm = new();
            algorithm.Execute(Grid);
            

            Cell start = Grid[Grid.Rows / 2, Grid.Columns / 2];
            (Grid as ColoredGrid).SetDistances(start.GetDistances());
        }
    }
}