using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class PrimDemo : MazeGenerator
    {
        public override void SetupGrid()
        {
            print("Call the other 2 Execute Context Menus instead.");
        }

        public override void Generate()
        {

        }

        [ContextMenu("Execute Simplified Prim's Algorithm")]
        void Execute1()
        {
            Grid = new ColoredGrid(Settings);

            SimplifiedPrim algorithm = new();
            Cell start = Grid[Grid.Rows / 2, Grid.Columns / 2];
            algorithm.ExecuteSync(Grid, start);

            Grid.SetDistances(start.GetDistances());

            Draw();
        }


        [ContextMenu("Execute True Prim's Algorithm")]
        void Execute2()
        {
            Grid = new ColoredGrid(Settings);

            TruePrim algorithm = new();
            Cell start = Grid[Grid.Rows / 2, Grid.Columns / 2];
            algorithm.ExecuteSync(Grid, start);

            Grid.SetDistances(start.GetDistances());

            Draw();
        }
    }
}