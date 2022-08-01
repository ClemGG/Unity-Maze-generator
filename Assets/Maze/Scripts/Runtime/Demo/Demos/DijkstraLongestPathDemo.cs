namespace Project.Procedural.MazeGeneration
{
    public class DijkstraLongestPathDemo : MazeGenerator
    {

        public override void SetupGrid()
        {
            Grid = new DistanceGrid(Settings);
        }

        //We run the GetDistances() algorith twice.
        //This is in cas the start Cell is somewhere in the middle of the maze.
        //Since we are looking for the longest path, the get first the cell farthest from the start
        //(likely somewhere on the edges of the maze), then we run it again to get the longest path.
        public override void Generate()
        {
            IGeneration genAlg = InterfaceFactory.GetGenerationAlgorithm(Settings);
            Cell start = Grid[0, 0];
            genAlg.ExecuteSync(Grid, start);

            Distances distances = start.GetDistances();
            (Cell newStart, int distance) = distances.Max();

            Distances newDistances = newStart.GetDistances();
            (Cell goal, int goalDistance) = newDistances.Max();
            (Grid as DistanceGrid).Distances = newDistances.PathTo(goal);
        }

        public override void Draw()
        {
            DrawMethod = new ConsoleDraw();
            DrawMethod.DrawSync(Grid);
        }
    }
}