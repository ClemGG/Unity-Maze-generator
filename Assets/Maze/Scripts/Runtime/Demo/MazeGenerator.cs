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
            Grid grid = new(GenerationSettings);

            IGeneration genAlg = InterfaceFactory.GetGenerationAlgorithm(GenerationSettings);
            genAlg.Execute(grid);

            grid.Braid(GenerationSettings.BraidRate);

            grid.DisplayGrid(GenerationSettings.DisplayMode);
        }
    }
}