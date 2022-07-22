using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class PrimDemo : MonoBehaviour
    {

        [field: SerializeField] private GenerationSettingsSO Settings { get; set; }
        private IDraw _drawMethod;


        [ContextMenu("Cleanup")]
        void Cleanup()
        {
            if (_drawMethod is not null)
            {
                _drawMethod.Cleanup();
            }
        }

        [ContextMenu("Execute Simplified Prim's Algorithm")]
        void Execute1()
        {
            var grid = new ColoredGrid(Settings);

            SimplifiedPrim algorithm = new();
            Cell start = grid[grid.Rows / 2, grid.Columns / 2];
            algorithm.Execute(grid, start);

            grid.SetDistances(start.GetDistances());

            SceneLoader.LoadSceneForDrawMode(Settings.DrawMode);
            _drawMethod = InterfaceFactory.GetDrawMode(Settings);
            _drawMethod.Draw(grid);
        }


        [ContextMenu("Execute True Prim's Algorithm")]
        void Execute2()
        {
            var grid = new ColoredGrid(Settings);

            TruePrim algorithm = new();
            Cell start = grid[grid.Rows / 2, grid.Columns / 2];
            algorithm.Execute(grid, start);

            grid.SetDistances(start.GetDistances());

            SceneLoader.LoadSceneForDrawMode(Settings.DrawMode);
            _drawMethod = InterfaceFactory.GetDrawMode(Settings);
            _drawMethod.Draw(grid);
        }
    }
}