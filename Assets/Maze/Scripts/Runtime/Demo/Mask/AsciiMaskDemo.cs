using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    //Warning: Not all generation & solving algos will be able to create & solve mazes with orphan (killed) cells
    public class AsciiMaskDemo : MonoBehaviour
    {
        [field: SerializeField] private GenerationSettingsSO GenerationSettings { get; set; }


        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            if (GenerationSettings.AsciiMask is null)
            {
                throw new("Error : The text file is missing.");
            }

            Mask m = Mask.FromText(GenerationSettings.AsciiMask.name);

            var grid = new MaskedGrid(m.Rows, m.Columns);
            grid.SetMask(m);
            RecursiveBacktracker algorithm = new();
            algorithm.Execute(grid);

            grid.DisplayGrid(DisplayMode.UIImage);
        }
    }
}