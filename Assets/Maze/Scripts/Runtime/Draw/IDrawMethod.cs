namespace Project.Procedural.MazeGeneration
{
    public interface IDrawMethod<in T>
    {
        void Draw(IDrawableGrid<T> grid);
        void Cleanup();
    }
}