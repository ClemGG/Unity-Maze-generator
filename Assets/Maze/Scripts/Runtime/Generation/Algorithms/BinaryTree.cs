using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{

    /* The Binary Tree Algorithm takes the northern and eastern neighbors of each Cell,
     * and if any, links them together at random.
     * This creates a Bias in the Maze where its northern and eastrn sides will be giant passages
     * with no walls.
     */
    public class BinaryTree : IGeneration
    {
        public GenerationProgressReport Report { get; set; } = new();

        public void ExecuteSync(IGrid grid, Cell start = null)
        {
            foreach (Cell cell in grid.EachCell())
            {
                List<Cell> neighbors = new();

                if (cell.North != null) neighbors.Add(cell.North);
                if (cell.East != null) neighbors.Add(cell.East);

                Cell neighbor = neighbors.Sample();

                if (neighbor != null) cell.Link(neighbor);
            }
        }


        public IEnumerator ExecuteAsync(IGrid grid, IProgress<GenerationProgressReport> progress, Cell start = null)
        {
            
            List<Cell> linkedCells = new();

            foreach (Cell cell in grid.EachCell())
            {
                List<Cell> neighbors = new();

                if (cell.North != null) neighbors.Add(cell.North);
                if (cell.East != null) neighbors.Add(cell.East);

                Cell neighbor = neighbors.Sample();

                if (neighbor != null) cell.Link(neighbor);

                linkedCells.Add(cell);
                Report.ProgressPercentage = (float)(linkedCells.Count * 100 / grid.Size()) / 100f;
                Report.UpdateTrackTime(Time.deltaTime);
                progress.Report(Report);
                yield return null;
            }
            
        }
    }
}