#if UNITY_EDITOR
using System.Threading.Tasks;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class ConsoleDraw : IDrawMethod<string>, IDrawMethodAsync<string>
    {

        public void Cleanup()
        {
            // This simply does "LogEntries.Clear()" the long way:
            var logEntries = System.Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
            var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            clearMethod.Invoke(null, null);
        }


        public void DrawSync(IDrawableGrid<string> grid)
        {
            Debug.Log(grid.ToString());
        }

        public async Task DrawAsync(IDrawableGrid<string> grid) 
        {
            await Task.Run(() =>
            {
                Debug.Log("async");
                DrawSync(grid);
            });
        }
    }
}

#endif