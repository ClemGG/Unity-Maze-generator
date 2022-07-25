namespace Project.Procedural.MazeGeneration
{
    public interface IDrawMethod
    {
        void Draw(IDrawableGrid grid);
        void Cleanup();
    }

    public interface IDrawMethod<in T> : IDrawMethod
    {
        void IDrawMethod.Draw(IDrawableGrid grid) => Draw(grid as IDrawableGrid<T>);
        void Draw(IDrawableGrid<T> grid);
    }
}