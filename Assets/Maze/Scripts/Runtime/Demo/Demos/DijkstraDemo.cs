namespace Project.Procedural.MazeGeneration
{
    public class DijkstraDemo : MazeGenerator
    {
        public override void SetupGrid()
        {
            Grid = new DistanceGrid(Settings);
        }

        public override void Generate()
        {
            IGeneration genAlg = InterfaceFactory.GetGenerationAlgorithm(Settings);
            Cell start = Grid[0, 0];
            genAlg.Execute(Grid, start);

            DrawMethod = new ConsoleDraw();
            Distances distances = start.GetDistances();
            (Grid as DistanceGrid).Distances = distances;
            DrawMethod.Draw(Grid);


            (Grid as DistanceGrid).Distances = distances.PathTo(Grid[Grid.Rows - 1, 0]);
            print("path from northwest corner to southwest corner:");
            DrawMethod.Draw(Grid);
        }

        public override void Draw()
        {

        }
    }
}