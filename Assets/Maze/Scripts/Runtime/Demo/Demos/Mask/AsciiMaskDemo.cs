using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    //Warning: Not all generation & solving algos will be able to create & solve mazes with orphan (killed) cells
    public class AsciiMaskDemo : MonoBehaviour
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
            if (Settings.AsciiMask is null)
            {
                throw new("Error : The text file is missing.");
            }

            Mask m = Mask.FromText(Settings.AsciiMask.name);

            var grid = new MaskedGrid(m.Rows, m.Columns);
            grid.SetMask(m);
            RecursiveBacktracker algorithm = new();
            algorithm.Execute(grid);

            SceneLoader.LoadSceneForDrawMode(Settings.DrawMode);
            _drawMethod = InterfaceFactory.GetDrawMode(Settings);
            _drawMethod.Draw(grid);
        }
    }
}