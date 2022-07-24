using System.Collections.Generic;

namespace Project.Procedural.MazeGeneration
{
    /* The Sidewinder Algorithm traverses each row from the bottom and randomly chooses 
     * at each step to either carve east from the current cell, 
     * or north from the current run of cells.
     */

    public class Sidewinder : IGeneration
    {
        public void Execute(IGrid grid, Cell start = null)
        {
            foreach (Cell[] row in grid.EachRow())
            {
                List<Cell> run = new();

                foreach (Cell cell in row)
                {
                    run.Add(cell);

                    bool atEasternBoundary = cell.East is null;
                    bool atNorthernBoundary = cell.North is null;
                    bool shouldCloseOut = atEasternBoundary || (!atNorthernBoundary && 2.Sample() == 0);

                    if (shouldCloseOut)
                    {
                        Cell member = run.Sample();
                        if (member.North is not null)
                        {
                            member.Link(member.North);
                        }
                        run.Clear();
                    }
                    else
                    {
                        cell.Link(cell.East);
                    }
                }
            }
        }
    }
}