
namespace Project.Procedural.MazeGeneration
{
    public interface IDemo<T>
    {
        GenerationSettingsSO Settings { get; set; }
        IGrid Grid { get; set; }
        IDrawMethod<T> DrawMethod { get; set; }

        void Cleanup();
        void Execute();
        void SetupGrid();
        void Generate();
        void Draw();
    }
}