namespace Project.Procedural.MazeGeneration
{
    public class MeshDemo : MazeGenerator
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
            Cell start = Grid[Grid.Rows / 2, Grid.Columns / 2];
            genAlg.Execute(Grid);

            

            Grid.SetDistances(start.GetDistances());
        }

        public override void Draw()
        {
            SceneLoader.LoadSceneForDrawMode(Settings.DrawMode);
            DrawMethod = InterfaceFactory.GetDrawMode(Settings);
            DrawMethod.Draw(Grid);
        }
    }
}