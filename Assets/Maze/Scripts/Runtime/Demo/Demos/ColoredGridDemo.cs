using System;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class ColoredGridDemo : MonoBehaviour
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

            IGeneration genAlg = InterfaceFactory.GetGenerationAlgorithm(Settings);
            Cell start = grid[grid.Rows / 2, grid.Columns / 2];
            genAlg.Execute(grid);
            grid.SetDistances(start.GetDistances());

            SceneLoader.LoadSceneForDrawMode(Settings.DrawMode);
            _drawMethod = InterfaceFactory.GetDrawMode(Settings);
            _drawMethod.Draw(grid);
        }
    }
}