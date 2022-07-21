using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    //Warning: Not all generation & solving algos will be able to create & solve mazes with orphan (killed) cells
    public class SimpleMaskDemo : MonoBehaviour
    {
        [field: SerializeField] private GenerationSettingsSO GenerationSettings { get; set; }



        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            Mask m = new(GenerationSettings);
            m[0, 0] = m[2, 2] = m[4, 4] = false;

            var grid = new MaskedGrid(GenerationSettings);
            grid.SetMask(m);

            RecursiveBacktracker algorithm = new();
            algorithm.Execute(grid);

            grid.DisplayGrid(DisplayMode.Print);
        }
    }
}