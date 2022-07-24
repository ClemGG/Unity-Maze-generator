namespace Project.Procedural.MazeGeneration
{
    public class MainMazeGenerator : MazeGenerator
    {
        public override void SetupGrid()
        {
            Grid = new ColoredGrid(Settings);
        }

        public override void Generate()
        {
            IGeneration genAlg = InterfaceFactory.GetGenerationAlgorithm(Settings);
            genAlg.Execute(Grid);

            
        }


    }
}