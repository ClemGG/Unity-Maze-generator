namespace Project.Procedural.MazeGeneration
{
    //Warning: Not all generation & solving algos will be able to create & solve mazes with orphan (killed) cells
    public class AsciiMaskDemo : MazeGenerator
    {
        public override void SetupGrid()
        {
            if (Settings.AsciiMask is null)
            {
                throw new("Error : The text file is missing.");
            }

            Mask m = Mask.FromText(Settings.AsciiMask.name);

            Grid = new MaskedGrid(m);
        }

        public override void Generate()
        {
            RecursiveBacktracker algorithm = new();
            algorithm.ExecuteSync(Grid);
        }
    }
}