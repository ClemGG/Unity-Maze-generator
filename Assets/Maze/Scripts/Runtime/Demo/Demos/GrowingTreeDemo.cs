namespace Project.Procedural.MazeGeneration
{
    public class GrowingTreeDemo : MazeGenerator
    {
        public override void SetupGrid()
        {
            Grid = new ColoredGrid(Settings);
        }

        public override void Generate()
        {
            GrowingTree algorithm = new(Settings);

            Cell start = Grid.RandomCell();
            algorithm.Execute(Grid, start);
            (Grid as ColoredGrid).SetDistances(start.GetDistances());
        }
    }
}