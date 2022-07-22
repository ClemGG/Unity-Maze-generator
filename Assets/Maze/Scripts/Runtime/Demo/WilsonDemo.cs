using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class WilsonDemo : MonoBehaviour
    {
        [field: SerializeField] private GenerationSettingsSO GenerationSettings { get; set; }


        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            var grid = new ColoredGrid(GenerationSettings);
            Wilson algorithm = new();
            algorithm.Execute(grid);

            Cell start = grid[grid.Rows / 2, grid.Columns / 2];
            grid.SetDistances(start.GetDistances());

            grid.DisplayGrid(DrawMode.UIImage);
        }
    }
}