
namespace Project.Procedural.MazeGeneration
{
    public interface IDemo
    {
        GenerationSettingsSO Settings { get; set; }
        IDrawableGrid Grid { get; set; }
        IDrawMethod DrawMethod { get; set; }


        void Cleanup();
        void Execute();
        void SetupGrid();
        void Generate();
        void Draw();
    }

}