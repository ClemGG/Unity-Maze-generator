using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    //Wilson is the opposite of Aldous-Broder:
    //AB starts fast and ends slow, W starts slow and ends fast.
    public class Wilson : IGeneration
    {

        public void ExecuteSync(IGrid grid, Cell start = null)
        {
            List<Cell> unvisited = new(grid.Size());
            foreach (Cell cell in grid.EachCell())
            {
                if (cell is null) continue;

                unvisited.Add(cell);
            }

            Cell first = unvisited.Sample();
            unvisited.Remove(first);

            while (unvisited.Count > 0)
            {
                Cell cell;
                do
                {
                    cell = unvisited.Sample();
                }
                while (cell is null);

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
        
        public IEnumerator ExecuteAsync(IGrid grid, IProgress<GenerationProgressReport> progress, Cell start = null)
        {
            GenerationProgressReport report = new();

            List<Cell> unvisited = new(grid.Size());
            foreach (Cell cell in grid.EachCell())
            {
                if (cell is null) continue;
                unvisited.Add(cell);
            }

            Cell first = unvisited.Sample();
            unvisited.Remove(first);

            while (unvisited.Count > 0)
            {
                Cell cell;
                do
                {
                    cell = unvisited.Sample();
                }
                while (cell is null);

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

                report.ProgressPercentage = (float)((grid.Size() - unvisited.Count) * 100 / grid.Size()) / 100f;
                report.UpdateTrackTime(Time.deltaTime);
                progress.Report(report);
                yield return null;
            }
        }
    }
}