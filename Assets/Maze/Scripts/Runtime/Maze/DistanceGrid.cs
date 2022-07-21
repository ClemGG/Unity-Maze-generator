using System;

namespace Project.Procedural.MazeGeneration
{
    public class DistanceGrid : Grid
    {
        public Distances Distances { get; set; }

        public DistanceGrid(int rows, int columns) : base(rows, columns)
        {
        }

        public DistanceGrid(GenerationSettingsSO generationSettings) : base(generationSettings)
        {
        }

        //This will display the distance on each cell traversed
        //by Dijkstra’s solving algorithm.
        protected override string ContentsOf(Cell cell)
        {
            if(Distances is not null && Distances[cell] != -1)
            {
                return Distances[cell].ToString();
            }
            return base.ContentsOf(cell);
        }
    }
}