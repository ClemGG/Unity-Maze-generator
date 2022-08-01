using System;
using System.Collections;

namespace Project.Procedural.MazeGeneration
{
    public class OneRoom : IGeneration
    {
        public void ExecuteSync(IGrid grid, Cell start = null)
        {
            grid.LinkAll();
        }


        public IEnumerator ExecuteAsync(IGrid grid, IProgress<GenerationProgressReport> progress, Cell start = null)
        {
            GenerationProgressReport report = new();
            report.StartTrackTime();

            grid.LinkAll();

            report.StopTrackTime();
            report.ProgressPercentage = 1f;
            progress.Report(report);
            yield return null;
        }
    }
}