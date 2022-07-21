using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    //Warning: Not all generation & solving algos will be able to create & solve mazes with orphan (killed) cells
    public class SimpleMaskDemo : MonoBehaviour
    {
        [field: SerializeField] private Vector2Int GridSize { get; set; } = new(4, 4);


#if UNITY_EDITOR

        private void OnValidate()
        {
            GridSize = new(Mathf.Clamp(GridSize.x, 1, 100), Mathf.Clamp(GridSize.y, 1, 100));
        }
#endif


        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            Mask m = new(GridSize.x, GridSize.y);
            m[0, 0] = m[2, 2] = m[4, 4] = false;

            var grid = new MaskedGrid(GridSize.x, GridSize.y);
            grid.SetMask(m);
            grid.Execute(GenerationType.RecursiveBacktracker);
            grid.DisplayGrid(DisplayMode.Print);
        }
    }
}