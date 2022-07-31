#if UNITY_EDITOR
using System;
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

        //Honestly, the progress report is not necessary here.
        //But we still do it to showcase how it works and to have at least something to show to the user.
        public async Task DrawAsync(IDrawableGrid<string> grid, IProgress<GenerationProgressReport> progress) 
        {
            GenerationProgressReport report = new();

            await Task.Run(() =>
            {
                DrawSync(grid);

                report.ProgressPercentage = 1f;
                progress.Report(report);
            });
        }
    }
}

#endif