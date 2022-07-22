namespace Project.Procedural.MazeGeneration
{
    //Warning: Not all generation & solving algos will be able to create & solve mazes with orphan (killed) cells
    public class KillCellsDemo : MazeGenerator
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
            Cell start = Grid[1, 1];
            RecursiveBacktracker algorithm = new();
            algorithm.Execute(Grid, start);

            (Grid as ColoredGrid).SetDistances(start.GetDistances());
        }

        public override void Draw()
        {
            SceneLoader.LoadSceneForDrawMode(Settings.DrawMode);
            DrawMethod = InterfaceFactory.GetDrawMode(Settings);
            DrawMethod.Draw(Grid);
        }


    }
}