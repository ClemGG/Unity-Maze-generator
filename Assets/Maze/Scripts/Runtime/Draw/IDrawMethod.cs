using System.Threading.Tasks;

namespace Project.Procedural.MazeGeneration
{
    public interface IDrawMethod
    {
        void DrawSync(IDrawableGrid grid);
        Task DrawAsync(IDrawableGrid grid);
        void Cleanup();
    }

    public interface IDrawMethod<in T> : IDrawMethod
    {
        void IDrawMethod.DrawSync(IDrawableGrid grid) => DrawSync(grid as IDrawableGrid<T>);
        void DrawSync(IDrawableGrid<T> grid);
    }

    public interface IDrawMethodAsync<in T> : IDrawMethod
    {
        Task IDrawMethod.DrawAsync(IDrawableGrid grid) => DrawAsync(grid as IDrawableGrid<T>);
        Task DrawAsync(IDrawableGrid<T> grid);
    }
}