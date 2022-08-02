
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    //The Aldous-Broder algorithm starts with a random Cell
    //and moves to a rando neighbor each at each tick.
    //Being completely random with no bias, this algorithm
    //can take a really long time to compute,
    //so it's best to use it in a small maze.
    public class AldousBroder : IGeneration
    {
        public GenerationProgressReport Report { get; set; } = new();

        public void ExecuteSync(IGrid grid, Cell start = null)
        {
            Cell cell = start ?? grid.RandomCell();
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



        public IEnumerator ExecuteAsync(IGrid grid, IProgress<GenerationProgressReport> progress, Cell start = null)
        {
            
            List<Cell> linkedCells = new();

            Cell cell = start ?? grid.RandomCell();
            int unvisited = grid.Size() - 1;

            while (unvisited > 0)
            {
                Cell neighbor = cell.Neighbors.Sample();

                if (neighbor.Links.Count == 0)
                {
                    cell.Link(neighbor);
                    unvisited--;

                    linkedCells.Add(cell);
                }

                cell = neighbor;


                Report.ProgressPercentage = (float)(linkedCells.Count * 100 / grid.Size()) / 100f;
                Report.UpdateTrackTime(Time.deltaTime);
                progress.Report(Report);
                yield return null;
            }
        }
    }
}