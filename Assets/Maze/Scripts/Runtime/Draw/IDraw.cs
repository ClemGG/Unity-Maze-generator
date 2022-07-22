namespace Project.Procedural.MazeGeneration
{
    public interface IDraw
    {
        void Draw(Grid grid);
        void Cleanup();
    }
}