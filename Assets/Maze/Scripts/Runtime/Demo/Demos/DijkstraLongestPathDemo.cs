namespace Project.Procedural.MazeGeneration
{
    public class DijkstraLongestPathDemo : MazeGenerator
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

            Distances distances = start.GetDistances();
            (Cell newStart, int distance) = distances.Max();

            var newDistances = newStart.GetDistances();
            (Cell goal, int goalDistance) = newDistances.Max();
            (Grid as DistanceGrid).Distances = newDistances.PathTo(goal);
        }
    }
}