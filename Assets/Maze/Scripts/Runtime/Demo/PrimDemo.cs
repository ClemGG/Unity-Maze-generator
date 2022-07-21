using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class PrimDemo : MonoBehaviour
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

        [ContextMenu("Execute Simplified Prim's Algorithm")]
        void Execute1()
        {
            var grid = new ColoredGrid(GridSize.x, GridSize.y);
            Cell start = grid[grid.Rows / 2, grid.Columns / 2];
            grid.Execute(GenerationType.SimplifiedPrim, start);

            grid.SetDistances(start.GetDistances());

            grid.DisplayGrid(DisplayMode.UIImage);
        }


        [ContextMenu("Execute True Prim's Algorithm")]
        void Execute2()
        {
            var grid = new ColoredGrid(GridSize.x, GridSize.y);
            Cell start = grid[grid.Rows / 2, grid.Columns / 2];
            grid.Execute(GenerationType.TruePrim, start);

            grid.SetDistances(start.GetDistances());

            grid.DisplayGrid(DisplayMode.UIImage);
        }
    }
}