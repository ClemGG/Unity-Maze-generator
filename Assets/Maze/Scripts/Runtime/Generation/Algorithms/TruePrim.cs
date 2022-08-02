using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    //The Simplified Prim gives a different cost to each passage between passages,
    //then selects the cells with the lowest cost.
    public class TruePrim : IGeneration
    {
        public GenerationProgressReport Report { get; set; } = new();

        public void ExecuteSync(IGrid grid, Cell start = null)
        {
            start ??= grid.RandomCell();

            List<Cell> active = new() { start };
            Dictionary<Cell, int> costs = new();

            foreach (Cell cell in grid.EachCell())
            {
                costs.Add(cell, 100.Sample());
            }


            while (active.Any())
            {
                //Get Cell with minimum cost
                Cell cell = null;
                int minCost = 100;

                foreach (Cell c in active)
                {
                    if (costs[c] < minCost)
                    {
                        minCost = costs[c];
                        cell = c;
                    }
                }

                Cell[] availableNeighbors = cell.Neighbors.Where(n => n.Links.Count == 0).ToArray();

                if (availableNeighbors.Any())
                {
                    Cell neighbor = null;
                    int minNCost = 100;
                    foreach (Cell c in availableNeighbors)
                    {
                        if (costs[c] < minNCost)
                        {
                            minNCost = costs[c];
                            neighbor = c;
                        }
                    }

                    cell.Link(neighbor);
                    active.Add(neighbor);
                }
                else
                {
                    active.Remove(cell);
                }
            }
        }



        public IEnumerator ExecuteAsync(IGrid grid, IProgress<GenerationProgressReport> progress, Cell start = null)
        {
            
            List<Cell> linkedCells = new();

            start ??= grid.RandomCell();

            List<Cell> active = new() { start };
            Dictionary<Cell, int> costs = new();

            foreach (Cell cell in grid.EachCell())
            {
                costs.Add(cell, 100.Sample());
            }


            while (active.Any())
            {
                //Get Cell with minimum cost
                Cell cell = null;
                int minCost = 100;

                foreach (Cell c in active)
                {
                    if (costs[c] < minCost)
                    {
                        minCost = costs[c];
                        cell = c;
                    }
                }

                Cell[] availableNeighbors = cell.Neighbors.Where(n => n.Links.Count == 0).ToArray();

                if (availableNeighbors.Any())
                {
                    Cell neighbor = null;
                    int minNCost = 100;
                    foreach (Cell c in availableNeighbors)
                    {
                        if (costs[c] < minNCost)
                        {
                            minNCost = costs[c];
                            neighbor = c;
                        }
                    }

                    cell.Link(neighbor);
                    active.Add(neighbor);

                    linkedCells.Add(cell);
                }
                else
                {
                    active.Remove(cell);
                }


                Report.ProgressPercentage = (float)(linkedCells.Count * 100 / grid.Size()) / 100f;
                Report.UpdateTrackTime(Time.deltaTime);
                progress.Report(Report);
                yield return null;
            }
        }
    }
}