using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class PrimDemo : MonoBehaviour
    {

        [field: SerializeField] private GenerationSettingsSO GenerationSettings { get; set; }


        [ContextMenu("Cleanup UI")]
        void CleanupUI()
        {
            OrthogonalMaze.CleanupUI();
        }

        [ContextMenu("Execute Simplified Prim's Algorithm")]
        void Execute1()
        {
            var grid = new ColoredGrid(GenerationSettings);

            SimplifiedPrim algorithm = new();
            Cell start = grid[grid.Rows / 2, grid.Columns / 2];
            algorithm.Execute(grid, start);

            grid.SetDistances(start.GetDistances());

            grid.DisplayGrid(DisplayMode.UIImage);
        }


        [ContextMenu("Execute True Prim's Algorithm")]
        void Execute2()
        {
            var grid = new ColoredGrid(GenerationSettings);

            TruePrim algorithm = new();
            Cell start = grid[grid.Rows / 2, grid.Columns / 2];
            algorithm.Execute(grid, start);

            grid.SetDistances(start.GetDistances());

            grid.DisplayGrid(DisplayMode.UIImage);
        }
    }
}