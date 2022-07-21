using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    //Warning: Not all generation & solving algos will be able to create & solve mazes with orphan (killed) cells
    public class KillCellsDemo : MonoBehaviour
    {
        [field: SerializeField] private Vector2Int GridSize { get; set; } = new(4, 4);


#if UNITY_EDITOR

        private void OnValidate()
        {
            GridSize = new(Mathf.Clamp(GridSize.x, 1, 100), Mathf.Clamp(GridSize.y, 1, 100));
        }
#endif

        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            var grid = new ColoredGrid(GridSize.x, GridSize.y);

            //We'll create orphan Cells in the northwest and southeast corners of the map,
            //which means they will have no connections with the rest of the map and will
            //be removed from the algos' computations.
            grid[0, 0].East.West = null;
            grid[0, 0].South.North = null;
            grid[GridSize.x - 1, GridSize.y - 1].West.East = null;
            grid[GridSize.x - 1, GridSize.y - 1].North.South = null;

            Cell start = grid[1, 1];
            grid.Execute(GenerationType.RecursiveBacktracker, start);

            grid.SetDistances(start.GetDistances());

            grid.DisplayGrid(DisplayMode.Print);
        }
    }
}