using System.Collections.Generic;

namespace Project.Procedural.MazeGeneration
{
    //Wilson is the opposite of Aldous-Broder:
    //AB starts fast and ends slow, W starts slow and ends fast.
    public static class Wilson
    {

        public static void Execute(Grid grid)
        {
            List<Cell> unvisited = new(grid.Size);
            foreach (Cell cell in grid.EachCell())
            {
                unvisited.Add(cell);
            }

            Cell first = unvisited.Sample();
            unvisited.Remove(first);

            while(unvisited.Count > 0)
            {
                Cell cell = unvisited.Sample();
                List<Cell> path = new() { cell };

                while (unvisited.Contains(cell))
                {
                    cell = cell.Neighbors.Sample();
                    int position = path.IndexOf(cell);

                    if (position == -1)
                    {
                        path.Add(cell);
                    }
                    else
                    {
                        path = path.GetRange(0, position + 1);
                    }
                }

                for (int i = 0; i < path.Count - 1; i++)
                {
                    path[i].Link(path[i + 1]);
                    unvisited.Remove(path[i]);
                }
            }
        }
    }
}