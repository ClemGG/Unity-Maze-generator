using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class MazeGenerator : MonoBehaviour
    {
        [field: SerializeField] private Vector2Int GridSize { get; set; } = new(4, 4);
        [field: SerializeField] private DisplayMode DisplayMode { get; set; } = DisplayMode.Print;
        [field: SerializeField] private GenerationType GenerationType { get; set; } = GenerationType.BinaryTree;


        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            Grid grid = new(GridSize.x, GridSize.y);
            grid.Execute(GenerationType);
            grid.DisplayGrid(DisplayMode);
        }
    }
}