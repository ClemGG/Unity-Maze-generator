using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class MazeGenerator : MonoBehaviour
    {
        [field: SerializeField] private DisplayMode DisplayMode { get; set; } = DisplayMode.Print;
        [field: SerializeField] private GenerationType GenerationType { get; set; } = GenerationType.BinaryTree;


        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            Grid grid = new(10, 10);
            grid.Execute(GenerationType);
            grid.DisplayGrid(DisplayMode);
        }
    }
}