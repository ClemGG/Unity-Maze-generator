using System;
using System.Collections;

namespace Project.Procedural.MazeGeneration
{
    public interface IGeneration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="start">If the algorithm doesn't need a starting Cell, it will be ignored.</param>
        public void ExecuteSync(IGrid grid, Cell start = null);
        public IEnumerator ExecuteAsync(IGrid grid, IProgress<GenerationProgressReport> progress, Cell start = null) { yield return null; }
    }
}