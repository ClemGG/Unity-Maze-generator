using System;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class ColoredGridDemo : MonoBehaviour
    {
        [field: SerializeField] private GenerationSettingsSO GenerationSettings { get; set; }



        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            var grid = new ColoredGrid(GenerationSettings);

            IGeneration genAlg = InterfaceFactory.GetGenerationAlgorithm(GenerationSettings);
            Cell start = grid[grid.Rows / 2, grid.Columns / 2];
            genAlg.Execute(grid);
            grid.SetDistances(start.GetDistances());

            grid.DisplayGrid(DisplayMode.UIImage);
        }
    }
}