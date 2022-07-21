using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class RecursiveBacktrackerDemo : MonoBehaviour
    {
        [field: SerializeField] private GenerationSettingsSO GenerationSettings { get; set; }


        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            var grid = new ColoredGrid(GenerationSettings);
            RecursiveBacktracker algorithm = new();
            Cell start = grid[grid.Rows / 2, grid.Columns / 2];
            algorithm.Execute(grid, start);

            grid.SetDistances(start.GetDistances());

            grid.DisplayGrid(DisplayMode.UIImage);
        }
    }
}