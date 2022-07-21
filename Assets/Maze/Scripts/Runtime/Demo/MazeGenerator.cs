using System;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class MazeGenerator : MonoBehaviour
    {
        [field: SerializeField] private GenerationSettingsSO GenerationSettings { get; set; }



        [ContextMenu("Cleanup UI")]
        void Cleanup()
        {
            OrthogonalMaze.CleanupUI();
        }

        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            Grid grid = new(GenerationSettings.GridSize.x, GenerationSettings.GridSize.y);

            //Dynamically creates the generation algorithm
            Type algType = Type.GetType($"Project.Procedural.MazeGeneration.{GenerationSettings.GenerationType}");
            IGeneration genAlg = (IGeneration)Activator.CreateInstance(algType, GenerationSettings);
            genAlg.Execute(grid);

            //grid.Execute(Settings.GenerationType);
            grid.DisplayGrid(GenerationSettings.DisplayMode);
        }
    }
}