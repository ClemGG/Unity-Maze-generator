
namespace Project.Procedural.MazeGeneration
{
    public class OneRoom : IGeneration
    {
        public void ExecuteSync(IGrid grid, Cell start = null)
        {
            grid.LinkAll();
        }
    }
}