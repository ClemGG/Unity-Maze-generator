using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class MazeGenerator3D : MonoBehaviour
    {
        [field: SerializeField] private GenerationType GenerationType { get; set; } = GenerationType.BinaryTree;
        [field: SerializeField] private Vector2 CellSize { get; set; } = new(3.5f, 3.5f);
        [field: SerializeField] private Vector2Int GridSize { get; set; } = new(4, 4);
        [field: SerializeField, Range(0f, 1f)] private float BraidRate { get; set; } = 1f;
        [field: SerializeField, Range(0f, .5f)] private float Inset { get; set; } = 0f;
        [field: SerializeField] private Texture2D ImageAsset { get; set; }
        [field: SerializeField] private string Extension { get; set; } = ".png";



#if UNITY_EDITOR

        private void OnValidate()
        {
            GridSize = new(Mathf.Clamp(GridSize.x, 1, 100), Mathf.Clamp(GridSize.y, 1, 100));
            OrthogonalMaze.MeshCellSize = CellSize;
        }
#endif


        [ContextMenu("Cleanup Mesh")]
        void CleanupUI()
        {
            OrthogonalMaze.CleanupMesh();
        }



        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            Grid grid;

            if (ImageAsset == null)
            {
                grid = new ColoredGrid(GridSize.x, GridSize.y);
            }
            else
            {
                Mask m = Mask.FromImgFile(ImageAsset, Extension);
                grid = new MaskedGrid(m.Rows, m.Columns);
                (grid as MaskedGrid).SetMask(m);
            }

            grid.Execute(GenerationType);
            grid.Braid(BraidRate);

            Cell start = grid[grid.Rows / 2, grid.Columns / 2];
            (grid as ColoredGrid).SetDistances(start.GetDistances());

            grid.DisplayGrid(DisplayMode.Mesh, Inset);
        }
    }
}