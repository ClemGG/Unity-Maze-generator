using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    [CreateAssetMenu(fileName = "New Generation Settings", menuName = "ScriptableObjects/Generation Settings", order = 1)]
    public class GenerationSettingsSO : ScriptableObject
    {

        [field: SerializeField] public DisplayMode DisplayMode { get; private set; } = DisplayMode.Print;

        [field: SerializeField] public GenerationType GenerationType { get; private set; } = GenerationType.BinaryTree;

        [field: SerializeField] public Vector2Int GridSize { get; private set; } = new(10, 10);

        [Tooltip("Used in 3D for the width and height of a Cell in the Mesh.")]
        [field: SerializeField] public Vector2Int CellSize { get; private set; } = new(5, 5);


        [Tooltip("Used by the Recursive Division Algorithm to stop the process if a room is smaller than this size.")]
        [field: SerializeField] public Vector2Int RoomSize { get; private set; } = new(5, 5);

        [Tooltip("Used by the Recursive Division Algorithm. If true, will create many more rooms than corridors in the maze.")]
        [field: SerializeField] public bool BiasTowardsRooms { get; private set; } = false;

        [Tooltip("Used by the Growing Tree Algorithm to select different ways to process the unvisited Cells.")]
        [field: SerializeField, Range(0, 3)] public int LambdaSelection { get; private set; } = 0;


        [Tooltip("Generate space between walls. This field is a percentage.")]
        [field: SerializeField, Range(0f, .5f)] public float Inset { get; private set; } = 0f;

        [Tooltip("Removes deadens to create loops in the maze. This field is a percentage.")]
        [field: SerializeField, Range(0f, 1f)] public float BraidRate { get; private set; } = 1f;

        [Tooltip("The Texture to use for masking the grid's cells.")]
        [field: SerializeField] public Texture2D ImageAsset { get; private set; }

        [Tooltip("The txt file to use for masking the grid's cells.")]
        [field: SerializeField] public TextAsset AsciiMask { get; private set; }

        [field: SerializeField] public string Extension { get; private set; } = ".png";


#if UNITY_EDITOR

        private void OnValidate()
        {
            GridSize = new(Mathf.Clamp(GridSize.x, 1, 100), Mathf.Clamp(GridSize.y, 1, 100));
            RoomSize = new(Mathf.Clamp(RoomSize.x, 1, 100), Mathf.Clamp(RoomSize.y, 1, 100));
        }

#endif
    }
}