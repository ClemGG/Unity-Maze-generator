using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class ImageMaskDemo : MonoBehaviour
    {

        [field: SerializeField] private GenerationSettingsSO GenerationSettings { get; set; }


        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            if (GenerationSettings.ImageAsset is null)
            {
                throw new("Error : The text file is missing.");
            }

            Mask m = Mask.FromImgFile(GenerationSettings.ImageAsset, GenerationSettings.Extension);

            var grid = new MaskedGrid(GenerationSettings);
            grid.SetMask(m);
            RecursiveBacktracker algorithm = new();
            algorithm.Execute(grid);

            grid.DisplayGrid(DisplayMode.UIImage);
        }
    }
}