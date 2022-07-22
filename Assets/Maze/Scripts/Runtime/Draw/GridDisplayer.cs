
namespace Project.Procedural.MazeGeneration
{

    public static class GridDisplayer
    {
        public static bool ShouldClearConsole { get; set; } = false;


        public static void DisplayGrid(this Grid grid, DrawMode mode, float inset = 0f)
        {
            switch (mode)
            {

            }
        }



//#if UNITY_EDITOR
//        static void ClearConsole()
//        {
//            // This simply does "LogEntries.Clear()" the long way:
//            var logEntries = System.Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
//            var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
//            clearMethod.Invoke(null, null);
//        }
//#endif


    }
}