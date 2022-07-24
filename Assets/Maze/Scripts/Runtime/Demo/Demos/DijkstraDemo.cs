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

            //This wil draw all the possible paths from this cell
            DrawMethod = new ConsoleDraw();
            Distances distances = start.GetDistances();
            (Grid as DistanceGrid).Distances = distances;
            DrawMethod.Draw(Grid);

            //Only from the start to the designated Cell
            (Grid as DistanceGrid).Distances = distances.PathTo(Grid[Grid.Rows - 1, 0]);
            print("path from northwest corner to southwest corner:");
            DrawMethod.Draw(Grid);
        }

        public override void Draw()
        {

        }
    }
}