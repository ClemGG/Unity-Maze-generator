
namespace Project.Procedural.MazeGeneration
{
    public class OneRoom : IGeneration
    {
        public void Execute(IGrid grid, Cell start = null)
        {
            grid.LinkAll();
        }
    }
}