namespace Project.Procedural.MazeGeneration
{
    public interface IGeneration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="start">If the algorithm doesn't need a starting Cell, it will be ignored.</param>
        public void Execute(Grid grid, Cell start = null);
    }
}