using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class RandomizedKruskalDemo : MonoBehaviour
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
            RandomizedKruskal algorithm = new();
            algorithm.Execute(grid);

            Cell start = grid[grid.Rows / 2, grid.Columns / 2];
            grid.SetDistances(start.GetDistances());

            grid.DisplayGrid(DrawMode.UIImage);
        }
    }
}