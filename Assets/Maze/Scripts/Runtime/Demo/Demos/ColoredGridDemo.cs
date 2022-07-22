namespace Project.Procedural.MazeGeneration
{
    public class ColoredGridDemo : MazeGenerator
    {
        public override void SetupGrid()
        {
            Grid = new ColoredGrid(Settings);

        }

        public override void Generate()
        {
            IGeneration genAlg = InterfaceFactory.GetGenerationAlgorithm(Settings);
            Cell start = Grid[Grid.Rows / 2, Grid.Columns / 2];
            genAlg.Execute(Grid);
            (Grid as ColoredGrid).SetDistances(start.GetDistances());
        }


    }
}