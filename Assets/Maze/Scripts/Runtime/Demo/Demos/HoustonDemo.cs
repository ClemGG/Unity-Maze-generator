namespace Project.Procedural.MazeGeneration
{
    public class HoustonDemo : MazeGenerator
    {
        public override void SetupGrid()
        {
            Grid = new ColoredGrid(Settings);
        }

        public override void Generate()
        {
            IGeneration algorithm = InterfaceFactory.GetGenerationAlgorithm(Settings);
            algorithm.ExecuteSync(Grid);

            Cell start = Grid[Grid.Rows / 2, Grid.Columns / 2];
            Grid.SetDistances(start.GetDistances());
        }
    }
}