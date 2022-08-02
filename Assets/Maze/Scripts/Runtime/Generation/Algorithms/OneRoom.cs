using System;
using System.Collections;

namespace Project.Procedural.MazeGeneration
{
    public class OneRoom : IGeneration
    {
        public GenerationProgressReport Report { get; set; } = new();

        public void ExecuteSync(IGrid grid, Cell start = null)
        {
            grid.LinkAll();
        }


        public IEnumerator ExecuteAsync(IGrid grid, IProgress<GenerationProgressReport> progress, Cell start = null)
        {
            
            Report.StartTrackTime();

            grid.LinkAll();

            Report.StopTrackTime();
            Report.ProgressPercentage = 1f;
            progress.Report(Report);
            yield return null;
        }
    }
}