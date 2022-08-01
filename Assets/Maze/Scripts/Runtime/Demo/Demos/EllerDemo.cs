namespace Project.Procedural.MazeGeneration
{
    public class EllerDemo : MazeGenerator
    {
        public override void SetupGrid()
        {
            Grid = new ColoredGrid(Settings);
        }

        public override void Generate()
        {
            Eller algorithm = new();
            algorithm.ExecuteSync(Grid);
            
            Cell start = Grid[Grid.Rows -1, Grid.Columns / 2];
            Grid.SetDistances(start.GetDistances());
        }
    }
}