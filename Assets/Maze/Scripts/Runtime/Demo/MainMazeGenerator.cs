namespace Project.Procedural.MazeGeneration
{
    public class MainMazeGenerator : MazeGenerator
    {
        public override void SetupGrid()
        {
            Grid = Settings.DrawMode == DrawMode.Console ? new DistanceGrid(Settings) : new ColoredGrid(Settings);
        }

        public override void Generate()
        {
            IGeneration genAlg = InterfaceFactory.GetGenerationAlgorithm(Settings);
            genAlg.Execute(Grid);


            Cell start = Grid[Grid.Rows / 2, Grid.Columns / 2];
            Grid.SetDistances(start.GetDistances());

        }


    }
}