using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class DijkstraDemo : MonoBehaviour
    {
        [field: SerializeField] private Vector2Int GridSize { get; set; } = new(4, 4);
        [field: SerializeField] private DisplayMode DisplayMode { get; set; } = DisplayMode.Print;
        [field: SerializeField] private GenerationType GenerationType { get; set; } = GenerationType.BinaryTree;


#if UNITY_EDITOR

        private void OnValidate()
        {
            GridSize = new(Mathf.Clamp(GridSize.x, 1, 100), Mathf.Clamp(GridSize.y, 1, 100));
        }
#endif

        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            var grid = new DistanceGrid(GridSize.x, GridSize.y);
            grid.Execute(GenerationType);

            Cell start = grid[0, 0];
            Distances distances = start.GetDistances();
            grid.Distances = distances;
            grid.DisplayGrid(DisplayMode);

            grid.Distances = distances.PathTo(grid[grid.Rows - 1, 0]);
            print("path from northwest corner to southwest corner:");
            grid.DisplayGrid(DisplayMode);
        }
    }
}