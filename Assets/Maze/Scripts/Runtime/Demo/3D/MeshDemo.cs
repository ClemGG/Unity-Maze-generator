using System;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class MeshDemo : MonoBehaviour
    {
        [field: SerializeField] private GenerationSettingsSO GenerationSettings { get; set; }

        [ContextMenu("Cleanup Mesh")]
        void CleanupUI()
        {
            OrthogonalMaze.CleanupMesh();
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
            Cell start = grid[grid.Rows / 2, grid.Columns / 2];
            genAlg.Execute(grid);

            grid.Braid(GenerationSettings.BraidRate);

            (grid as ColoredGrid).SetDistances(start.GetDistances());

            grid.DisplayGrid(DrawMode.Mesh, GenerationSettings.Inset);
        }
    }
}