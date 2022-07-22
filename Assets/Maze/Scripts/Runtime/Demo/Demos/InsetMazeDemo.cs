namespace Project.Procedural.MazeGeneration
{
    public class InsetMazeDemo : MazeGenerator
    {
        public override void SetupGrid()
        {
            if (Settings.ImageAsset == null)
            {
                Grid = new ColoredGrid(Settings);
            }
            else
            {
                Mask m = Mask.FromImgFile(Settings.ImageAsset, Settings.Extension);
                Grid = new MaskedGrid(m.Rows, m.Columns);
                (Grid as MaskedGrid).SetMask(m);
            }
        }

        public override void Generate()
        {
            IGeneration genAlg = InterfaceFactory.GetGenerationAlgorithm(Settings);
            genAlg.Execute(Grid);

            Grid.Braid(Settings.BraidRate);

            Cell start = Grid[Grid.Rows / 2, Grid.Columns / 2];
            (Grid as ColoredGrid).SetDistances(start.GetDistances());
        }
    }
}