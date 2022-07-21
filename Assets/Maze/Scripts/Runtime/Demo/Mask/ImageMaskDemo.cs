using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class ImageMaskDemo : MonoBehaviour
    {
        [field: SerializeField] private Texture2D ImageAsset { get; set; }
        [field: SerializeField] private string Extension { get; set; } = ".png";


        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            if (ImageAsset is null)
            {
                throw new("Error : The text file is missing.");
            }

            Mask m = Mask.FromImgFile(ImageAsset, Extension);

            var grid = new MaskedGrid(m.Rows, m.Columns);
            grid.SetMask(m);
            grid.Execute(GenerationType.RecursiveBacktracker);
            grid.DisplayGrid(DisplayMode.UIImage);
        }
    }
}