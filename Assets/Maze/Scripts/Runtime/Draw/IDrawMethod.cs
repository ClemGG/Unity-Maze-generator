using System;
using System.Collections;

namespace Project.Procedural.MazeGeneration
{
    public interface IDrawMethod
    {
        GenerationProgressReport Report { get; set; }

        void DrawSync(IDrawableGrid grid);

        /* Not really async, since it uses Coroutines.
         * Unity cannot instantiate GameObjects in a different thread,
         * so we use these instead.
         */
        IEnumerator DrawAsync(IDrawableGrid grid, IProgress<GenerationProgressReport> progress);
        void Cleanup();
    }

    public interface IDrawMethod<in T> : IDrawMethod
    {
        void IDrawMethod.DrawSync(IDrawableGrid grid) => DrawSync(grid as IDrawableGrid<T>);
        void DrawSync(IDrawableGrid<T> grid);
    }

    public interface IDrawMethodAsync<in T> : IDrawMethod
    {
        IEnumerator IDrawMethod.DrawAsync(IDrawableGrid grid, IProgress<GenerationProgressReport> progress) => 
                                    DrawAsync(grid as IDrawableGrid<T>, progress);
        IEnumerator DrawAsync(IDrawableGrid<T> grid, IProgress<GenerationProgressReport> progress);
    }
}