using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    //Warning: Not all generation & solving algos will be able to create & solve mazes with orphan (killed) cells
    public class KillCellsDemo : MonoBehaviour
    {

        [field: SerializeField] private GenerationSettingsSO GenerationSettings { get; set; }


        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            var grid = new ColoredGrid(GenerationSettings);

            //We'll create orphan Cells in the northwest and southeast corners of the map,
            //which means they will have no connections with the rest of the map and will
            //be removed from the algos' computations.
            grid[0, 0].East.West = null;
            grid[0, 0].South.North = null;
            grid[GenerationSettings.GridSize.x - 1, GenerationSettings.GridSize.y - 1].West.East = null;
            grid[GenerationSettings.GridSize.x - 1, GenerationSettings.GridSize.y - 1].North.South = null;

            Cell start = grid[1, 1];
            RecursiveBacktracker algorithm = new();
            algorithm.Execute(grid, start);

            grid.SetDistances(start.GetDistances());

            grid.DisplayGrid(DrawMode.Console);
        }
    }
}