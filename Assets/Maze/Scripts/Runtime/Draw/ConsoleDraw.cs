#if UNITY_EDITOR
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class ConsoleDraw : IDraw
    {
        private bool ShouldClearConsole { get; set; }

        public ConsoleDraw(bool shouldClearConsole)
        {
            ShouldClearConsole = shouldClearConsole;
        }



        public void Draw(Grid grid)
        {
            if (ShouldClearConsole) ClearConsole();
            Debug.Log(grid.ToString());
        }


        static void ClearConsole()
        {
            // This simply does "LogEntries.Clear()" the long way:
            var logEntries = System.Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
            var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            clearMethod.Invoke(null, null);
        }
    }
}

#endif