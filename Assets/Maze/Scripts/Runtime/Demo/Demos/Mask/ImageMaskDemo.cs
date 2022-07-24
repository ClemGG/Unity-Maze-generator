namespace Project.Procedural.MazeGeneration
{
    public class ImageMaskDemo : MazeGenerator
    {

        public override void SetupGrid()
        {
            if (Settings.ImageAsset is null)
            {
                throw new("Error : The text file is missing.");
            }

            Mask m = Mask.FromImgFile(Settings.ImageAsset, Settings.Extension);

            Grid = new MaskedGrid(Settings);
            (Grid as MaskedGrid).SetMask(m);
        }

        public override void Generate()
        {
            RecursiveBacktracker algorithm = new();
            algorithm.Execute(Grid);
            
        }

    }
}