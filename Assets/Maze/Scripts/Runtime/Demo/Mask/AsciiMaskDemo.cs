using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    //Warning: Not all generation & solving algos will be able to create & solve mazes with orphan (killed) cells
    public class AsciiMaskDemo : MonoBehaviour
    {
        [field: SerializeField] private TextAsset AsciiMask { get; set; }


        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            if (AsciiMask is null)
            {
                throw new("Error : The text file is missing.");
            }

            Mask m = Mask.FromText(AsciiMask.name);

            var grid = new MaskedGrid(m.Rows, m.Columns);
            grid.SetMask(m);
            grid.Execute(GenerationType.RecursiveBacktracker);
            grid.DisplayGrid(DisplayMode.UIImage);
        }
    }
}