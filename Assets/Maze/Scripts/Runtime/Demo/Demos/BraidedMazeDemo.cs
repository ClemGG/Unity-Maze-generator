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
            algorithm.ExecuteSync(Grid);
            

            Cell start = Grid[Grid.Rows / 2, Grid.Columns / 2];
            Grid.SetDistances(start.GetDistances());
        }
    }
}