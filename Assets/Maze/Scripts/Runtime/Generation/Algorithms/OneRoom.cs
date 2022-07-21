
namespace Project.Procedural.MazeGeneration
{
    public class OneRoom : IGeneration
    {
        public void Execute(Grid grid, Cell start = null)
        {
            grid.LinkAll();
        }
    }
}