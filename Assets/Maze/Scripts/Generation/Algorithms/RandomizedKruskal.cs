using System.Collections.Generic;

namespace Project.Procedural.MazeGeneration
{
    //Kruskal generates weights between passages and merge Cells with the lowest weight
    //on the grid in their own set.
    public static class RandomizedKruskal
    {
        private class State
        {
            public List<Cell[]> Neighbors { get; }
            public Dictionary<Cell, int> SetForCell { get; }
            public Dictionary<int, List<Cell>> CellsInSet { get; }
            public Grid Grid { get; }

            public State(Grid grid)
            {
                Grid = grid;
                Neighbors = new();
                SetForCell = new();
                CellsInSet = new();

                foreach (Cell cell in grid.EachCell())
                {
                    int set = SetForCell.Count;
                    SetForCell.Add(cell, set);
                    CellsInSet.Add(set, new() { cell });

                    if (cell.South is not null)
                    {
                        Neighbors.Add(new Cell[] { cell, cell.South });
                    }
                    if (cell.East is not null)
                    {
                        Neighbors.Add(new Cell[] { cell, cell.East });
                    }
                }
            }

            public bool CanMerge(Cell left, Cell right)
            {
                return SetForCell[left] != SetForCell[right];
            }

            public void Merge(Cell left, Cell right)
            {
                left.Link(right);
                int winner = SetForCell[left];
                int loser = SetForCell[right];
                List<Cell> losers = CellsInSet[loser];
                if(losers.Count == 0)
                {
                    losers.Add(right);
                }

                for (int i = 0; i < losers.Count; i++)
                {
                    CellsInSet[winner].Add(losers[i]);
                    SetForCell[losers[i]] = winner;
                }

                CellsInSet.Remove(loser);
            }

            
        }


        public static void Execute(Grid grid)
        {
            State state = new(grid);

            List<Cell[]> shuffledNeighbors = state.Neighbors.Shuffle();
            while (shuffledNeighbors.Count > 0)
            {
                Cell[] pair = shuffledNeighbors[shuffledNeighbors.Count - 1];
                shuffledNeighbors.Remove(pair);

                if (state.CanMerge(pair[0], pair[1]))
                {
                    state.Merge(pair[0], pair[1]);
                }
            }
        }
    }
}