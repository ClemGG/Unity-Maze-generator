namespace Project.Procedural.MazeGeneration
{
    public class ImageMaskDemo : MazeGenerator
    {

        public override void SetupGrid()
        {
            if (Settings.ImageMask is null)
            {
                throw new("Error : The text file is missing.");
            }

            Mask m = Mask.FromImgFile(Settings.ImageMask, Settings.Extension);

            Grid = new MaskedGrid(m);
        }

        public override void Generate()
        {
            RecursiveBacktracker algorithm = new();
            algorithm.ExecuteSync(Grid);

            Cell start = Grid.RandomCell();
            Grid.SetDistances(start.GetDistances());

        }

    }
}