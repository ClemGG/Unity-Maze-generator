#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
#endif

namespace Project.Procedural.MazeGeneration
{
    public static class SceneLoader
    {
        public static void LoadSceneForDrawMode(DrawMode drawMode)
        {
#if UNITY_EDITOR

            //Loads the appropriate scene depending on the DrawMode
            switch (drawMode)
            {
                case DrawMode.UIImage:
                    EditorSceneManager.CloseScene(EditorSceneManager.GetSceneByName("3D Maze"), false);
                    EditorSceneManager.OpenScene("Assets/Maze/Scenes/2D Maze.unity", OpenSceneMode.Additive);
                    break;

                case DrawMode.Mesh:
                    EditorSceneManager.CloseScene(EditorSceneManager.GetSceneByName("2D Maze"), false);
                    EditorSceneManager.OpenScene("Assets/Maze/Scenes/3D Maze.unity", OpenSceneMode.Additive);
                    break;
            }
#endif
        }
    }
}
