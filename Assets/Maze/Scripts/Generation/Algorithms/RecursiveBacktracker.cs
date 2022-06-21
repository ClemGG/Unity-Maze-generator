using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public static class RecursiveBacktracker
    {
        public static void Execute(Grid grid)
        {
            Stack<Cell> stack = new();
            Cell start = grid.RandomCell;
            stack.Push(start);

            while(stack.Count > 0)
            {
                Cell current = stack.Peek();
                List<Cell> neighbors = new();
                for (int i = 0; i < current.Neighbors.Count; i++)
                {
                    Cell unvNeighbor = current.Neighbors[i];
                    if (unvNeighbor.Links.Count == 0)
                    {
                        neighbors.Add(unvNeighbor);
                    }
                }

                if(neighbors.Count == 0)
                {
                    stack.Pop();
                }
                else
                {
                    Cell neighbor = neighbors.Sample();
                    current.Link(neighbor);
                    stack.Push(neighbor);
                }
            }
        }
    }
}