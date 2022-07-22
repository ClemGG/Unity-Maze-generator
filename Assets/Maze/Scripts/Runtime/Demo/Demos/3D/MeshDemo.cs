using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class MeshDemo : MonoBehaviour
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
            Grid grid;

            if (Settings.ImageAsset == null)
            {
                grid = new ColoredGrid(Settings);
            }
            else
            {
                Mask m = Mask.FromImgFile(Settings.ImageAsset, Settings.Extension);
                grid = new MaskedGrid(m.Rows, m.Columns);
                (grid as MaskedGrid).SetMask(m);
            }

            IGeneration genAlg = InterfaceFactory.GetGenerationAlgorithm(Settings);
            Cell start = grid[grid.Rows / 2, grid.Columns / 2];
            genAlg.Execute(grid);

            grid.Braid(Settings.BraidRate);

            (grid as ColoredGrid).SetDistances(start.GetDistances());

            SceneLoader.LoadSceneForDrawMode(Settings.DrawMode);
            _drawMethod = InterfaceFactory.GetDrawMode(Settings);
            _drawMethod.Draw(grid);
        }
    }
}