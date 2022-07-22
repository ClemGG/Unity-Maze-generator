using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace Project.Procedural.MazeGeneration
{
    public class MazeGenerator : MonoBehaviour
    {
        [field: SerializeField] private GenerationSettingsSO Settings { get; set; }
        private IDraw _drawMethod;


        [ContextMenu("Cleanup")]
        void Cleanup()
        {
            if(_drawMethod is not null)
            {
                _drawMethod.Cleanup();
            }
        }

        [ContextMenu("Execute Generation Algorithm")]
        void Execute()
        {
            Grid grid = new(Settings);

            IGeneration genAlg = InterfaceFactory.GetGenerationAlgorithm(Settings);
            genAlg.Execute(grid);

            grid.Braid(Settings.BraidRate);


#if UNITY_EDITOR
            //Loads the appropriate scene depending on the DrawMode
            switch (Settings.DrawMode)
            {
                case DrawMode.UIImage:
                    EditorSceneManager.CloseScene(EditorSceneManager.GetSceneByName("3D Maze"), false);
                    EditorSceneManager.OpenScene("2D Maze");
                    break;

                case DrawMode.Mesh:
                    EditorSceneManager.CloseScene(EditorSceneManager.GetSceneByName("2D Maze"), false);
                    EditorSceneManager.OpenScene("3D Maze");
                    break;
            }
#endif

            _drawMethod = InterfaceFactory.GetDrawMode(Settings);
            _drawMethod.Draw(grid);
        }
    }
}