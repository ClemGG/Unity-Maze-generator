using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class MazeGenerator : MonoBehaviour
    {
        [field: SerializeField] private GenerationSettingsSO Settings { get; set; }



        [ContextMenu("Cleanup UI")]
        void Cleanup()
        {
            OrthogonalMaze.CleanupUI();
        }

        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            Grid grid = new(Settings.GridSize.x, Settings.GridSize.y);

            //Dynamically creates the generation algorithm
            //Type algType = Type.GetType($"Project.Procedural.MazeGeneration.{GenerationType}");
            //IGeneration genAlg = (IGeneration)Activator.CreateInstance(algType);
            //genAlg.Execute(grid);

            grid.Execute(Settings.GenerationType);
            grid.DisplayGrid(Settings.DisplayMode);
        }
    }
}