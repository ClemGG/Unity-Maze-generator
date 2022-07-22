using System;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class InsetMazeDemo : MonoBehaviour
    {
        [field: SerializeField] private GenerationSettingsSO GenerationSettings { get; set; }




        [ContextMenu("Cleanup UI")]
        void CleanupUI()
        {
            OrthogonalMaze.CleanupUI();
        }


        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            Grid grid;

            if (GenerationSettings.ImageAsset == null)
            {
                grid = new ColoredGrid(GenerationSettings);
            }
            else
            {
                Mask m = Mask.FromImgFile(GenerationSettings.ImageAsset, GenerationSettings.Extension);
                grid = new MaskedGrid(m.Rows, m.Columns);
                (grid as MaskedGrid).SetMask(m);
            }


            IGeneration genAlg = InterfaceFactory.GetGenerationAlgorithm(GenerationSettings);
            genAlg.Execute(grid);

            grid.Braid(GenerationSettings.BraidRate);

            Cell start = grid[grid.Rows / 2, grid.Columns / 2];
            (grid as ColoredGrid).SetDistances(start.GetDistances());


            grid.DisplayGrid(DrawMode.UIImage, GenerationSettings.Inset);
        }
    }
}