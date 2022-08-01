namespace Project.Procedural.MazeGeneration
{
    public class RecursiveDivisionDemo : MazeGenerator
    {
        public override void SetupGrid()
        {
            Grid = new ColoredGrid(Settings);

            //We'll create orphan Cells in the northwest and southeast corners of the map,
            //which means they will have no connections with the rest of the map and will
            //be removed from the algos' computations.
            Grid[0, 0].East.West = null;
            Grid[0, 0].South.North = null;
            Grid[Settings.GridSize.x - 1, Settings.GridSize.y - 1].West.East = null;
            Grid[Settings.GridSize.x - 1, Settings.GridSize.y - 1].North.South = null;
        }

        public override void Generate()
        {
            RecursiveDivision genAlg = new(Settings);
            genAlg.ExecuteSync(Grid);

            Cell start = Grid[Grid.Rows / 2, Grid.Columns / 2];
            Grid.SetDistances(start.GetDistances());
        }
    }
}