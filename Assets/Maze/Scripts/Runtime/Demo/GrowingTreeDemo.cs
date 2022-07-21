using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class GrowingTreeDemo : MonoBehaviour
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

            GrowingTree algorithm = new(GenerationSettings);

            Cell start = grid.RandomCell();
            algorithm.Execute(grid, start);
            grid.SetDistances(start.GetDistances());

            grid.DisplayGrid(DisplayMode.UIImage);
        }
    }
}