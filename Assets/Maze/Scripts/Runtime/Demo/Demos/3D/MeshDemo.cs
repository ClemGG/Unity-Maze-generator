namespace Project.Procedural.MazeGeneration
{
    public class MeshDemo : MazeGenerator
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
            Cell start = Grid[Grid.Rows / 2, Grid.Columns / 2];
            genAlg.ExecuteSync(Grid);

            

            Grid.SetDistances(start.GetDistances());
        }

        public override void Draw()
        {
            SceneLoader.LoadSceneForDrawMode(Settings.DrawMode);
            DrawMethod = InterfaceFactory.GetDrawMode(Settings);
            DrawMethod.DrawSync(Grid);
        }
    }
}