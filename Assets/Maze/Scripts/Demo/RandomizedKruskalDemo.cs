using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class RandomizedKruskalDemo : MonoBehaviour
    {
        [field: SerializeField] private Vector2Int GridSize { get; set; } = new(4, 4);


#if UNITY_EDITOR

        private void OnValidate()
        {
            GridSize = new(Mathf.Clamp(GridSize.x, 1, 100), Mathf.Clamp(GridSize.y, 1, 100));
        }
#endif


        [ContextMenu("Cleanup UI")]
        void CleanupUI()
        {
            OrthogonalMaze.CleanupUI();
        }

        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            var grid = new ColoredGrid(GridSize.x, GridSize.y);
            grid.Execute(GenerationType.RandomizedKruskal);

            Cell start = grid[grid.Rows / 2, grid.Columns / 2];
            grid.SetDistances(start.GetDistances());

            grid.DisplayGrid(DisplayMode.UIImage);
        }
    }
}