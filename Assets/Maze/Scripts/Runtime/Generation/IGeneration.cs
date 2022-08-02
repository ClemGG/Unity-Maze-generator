using System;
using System.Collections;

namespace Project.Procedural.MazeGeneration
{
    public interface IGeneration
    {
        GenerationProgressReport Report { get; set; }

        /// <param name="start">If the algorithm doesn't need a starting Cell, it will be ignored.</param>
        void ExecuteSync(IGrid grid, Cell start = null);
        IEnumerator ExecuteAsync(IGrid grid, IProgress<GenerationProgressReport> progress, Cell start = null) { yield return null; }
    }
}