using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class BraidedMazeDemo : MonoBehaviour
    {

        [field: SerializeField] private GenerationSettingsSO GenerationSettings { get; set; }



        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            var grid = new ColoredGrid(GenerationSettings);
            RecursiveBacktracker algorithm = new();
            algorithm.Execute(grid);
            grid.Braid(GenerationSettings.BraidRate);

            Cell start = grid[grid.Rows / 2, grid.Columns / 2];
            grid.SetDistances(start.GetDistances());

            grid.DisplayGrid(DrawMode.UIImage);
        }
    }
}