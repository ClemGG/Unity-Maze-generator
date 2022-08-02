namespace Project.Procedural.MazeGeneration
{
    //Warning: Not all generation & solving algos will be able to create & solve mazes with orphan (killed) cells
    public class SimpleMaskDemo : MazeGenerator
    {
        public override void SetupGrid()
        {
            Mask m = new(Settings);
            m[0, 0] = m[2, 2] = m[4, 4] = false;

            Grid = new MaskedGrid(m);
        }

        public override void Generate()
        {
            RecursiveBacktracker algorithm = new();
            algorithm.ExecuteSync(Grid);
        }
    }
}