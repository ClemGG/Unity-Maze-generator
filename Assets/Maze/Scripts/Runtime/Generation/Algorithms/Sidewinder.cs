using System.Collections.Generic;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    /* The Sidewinder Algorithm traverses each row from the bottom and randomly chooses 
     * at each step to either carve east from the current cell, 
     * or north from the current run of cells.
     */

    public static class Sidewinder
    {
        private static List<Cell> _run { get; set; }

        public static void Execute(Grid grid)
        {
            foreach (Cell[] row in grid.EachRow())
            {
                _run = new();

                foreach (Cell cell in row)
                {
                    _run.Add(cell);

                    bool atEasternBoundary = cell.East is null;
                    bool atNorthernBoundary = cell.North is null;
                    bool shouldCloseOut = atEasternBoundary || (!atNorthernBoundary && 2.Sample() == 0);

                    if (shouldCloseOut)
                    {
                        Cell member = _run.Sample();
                        if (member.North is not null)
                        {
                            member.Link(member.North);
                        }
                        _run.Clear();
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