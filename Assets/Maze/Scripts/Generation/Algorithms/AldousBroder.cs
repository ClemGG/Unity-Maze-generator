
namespace Project.Procedural.MazeGeneration
{
    //The Aldous-Broder algorithm starts with a random Cell
    //and moves to a rando neighbor each at each tick.
    //Being completely random with no bias, this algorithm
    //can take a really long time to compute,
    //so it's best to use it in a small maze.
    public static class AldousBroder
    {
        public static void Execute(Grid grid)
        {

            Cell cell = grid.RandomCell();
            int unvisited = grid.Size() - 1;

            while(unvisited > 0)
            {
                Cell neighbor = cell.Neighbors.Sample();

                if(neighbor.Links.Count == 0)
                {
                    cell.Link(neighbor);
                    unvisited--;
                }

                cell = neighbor;
            }

        }
    }
}