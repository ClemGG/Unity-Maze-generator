using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class GrowingTreeDemo : MonoBehaviour
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

        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            var grid = new ColoredGrid(Settings);

            GrowingTree algorithm = new(Settings);

            Cell start = grid.RandomCell();
            algorithm.Execute(grid, start);
            grid.SetDistances(start.GetDistances());

            SceneLoader.LoadSceneForDrawMode(Settings.DrawMode);
            _drawMethod = InterfaceFactory.GetDrawMode(Settings);
            _drawMethod.Draw(grid);
        }
    }
}