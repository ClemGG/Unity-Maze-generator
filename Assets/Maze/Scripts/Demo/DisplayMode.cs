using UnityEngine;

namespace Project.Procedural.MazeGeneration
{

    public enum DisplayMode : byte
    {
        Print,
        ASCII,
        UI,
        Sprite,
        ThreeD,
    }


    public static class GridDisplayer
    {
        public static void DisplayGrid(this Grid grid, DisplayMode mode)
        {
            switch (mode)
            {
#if UNITY_EDITOR
                case DisplayMode.Print:
                    ClearConsole();
                    Debug.Log(grid.ToString());
                    break;
#endif
                case DisplayMode.UI:
                    DisplayOnUI(grid);
                    break;
            }
        }



#if UNITY_EDITOR
        static void ClearConsole()
        {
            // This simply does "LogEntries.Clear()" the long way:
            var logEntries = System.Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
            var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            clearMethod.Invoke(null, null);
        }
#endif


        private static void DisplayOnUI(Grid grid)
        {
            Canvas canvas = Object.FindObjectOfType<Canvas>();
        }
    }
}