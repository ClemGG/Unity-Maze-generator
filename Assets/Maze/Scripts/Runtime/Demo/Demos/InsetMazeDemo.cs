namespace Project.Procedural.MazeGeneration
{
    public class InsetMazeDemo : MazeGenerator
    {
        public override void SetupGrid()
        {
            if (Settings.ImageMask == null)
            {
                Grid = new ColoredGrid(Settings);
            }
            else
            {
                Mask m = Mask.FromImgFile(Settings.ImageMask, Settings.Extension);
                Grid = new MaskedGrid(m);
            }
        }

        public override void Generate()
        {
            IGeneration genAlg = InterfaceFactory.GetGenerationAlgorithm(Settings);
            genAlg.ExecuteSync(Grid);

            

            Cell start = Grid[Grid.Rows / 2, Grid.Columns / 2];
            Grid.SetDistances(start.GetDistances());
        }
    }
}