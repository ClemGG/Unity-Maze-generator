using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class EllerDemo : MonoBehaviour
    {
        [field: SerializeField] private GenerationSettingsSO GenerationSettings { get; set; }



        [ContextMenu("Cleanup UI")]
        void CleanupUI()
        {
            OrthogonalMaze.CleanupUI();
        }

        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            var grid = new ColoredGrid(GenerationSettings);
            Eller algorithm = new();
            algorithm.Execute(grid);

            Cell start = grid[grid.Rows -1, grid.Columns / 2];
            grid.SetDistances(start.GetDistances());

            grid.DisplayGrid(DisplayMode.UIImage);
        }
    }
}