namespace Project.Procedural.MazeGeneration
{
    public class HuntAndKillDemo : MazeGenerator
    {
        public override void SetupGrid()
        {
            Grid = new ColoredGrid(Settings);
        }

        public override void Generate()
        {
            HuntAndKill algorithm = new();
            algorithm.Execute(Grid);

            Cell start = Grid[Grid.Rows / 2, Grid.Columns / 2];
            Grid.SetDistances(start.GetDistances());
        }
    }
}