namespace Project.Procedural.MazeGeneration
{
    public class RecursiveBacktrackerDemo : MazeGenerator
    {
        public override void SetupGrid()
        {
            Grid = new ColoredGrid(Settings);
        }

        public override void Generate()
        {
            RecursiveBacktracker algorithm = new();
            Cell start = Grid[Grid.Rows / 2, Grid.Columns / 2];
            algorithm.ExecuteSync(Grid, start);
        }
    }
}