
namespace Project.Procedural.MazeGeneration
{
    public interface IDemo
    {
        GenerationSettingsSO Settings { get; set; }
        Grid Grid { get; set; }
        IDraw DrawMethod { get; set; }

        void Cleanup();
        void Execute();
        void SetupGrid();
        void Generate();
        void Draw();
    }
}