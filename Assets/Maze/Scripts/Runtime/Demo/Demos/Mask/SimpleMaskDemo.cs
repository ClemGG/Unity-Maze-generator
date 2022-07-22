using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    //Warning: Not all generation & solving algos will be able to create & solve mazes with orphan (killed) cells
    public class SimpleMaskDemo : MonoBehaviour
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
            Mask m = new(Settings);
            m[0, 0] = m[2, 2] = m[4, 4] = false;

            var grid = new MaskedGrid(Settings);
            grid.SetMask(m);

            RecursiveBacktracker algorithm = new();
            algorithm.Execute(grid);

            SceneLoader.LoadSceneForDrawMode(Settings.DrawMode);
            _drawMethod = InterfaceFactory.GetDrawMode(Settings);
            _drawMethod.Draw(grid);
        }
    }
}