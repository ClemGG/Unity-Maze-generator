using System;

namespace Project.Procedural.MazeGeneration
{
    public class DistanceGrid : Grid, IDistanceGrid
    {
        public Distances Distances { get; set; }

        public DistanceGrid(int rows, int columns) : base(rows, columns)
        {
        }

        public DistanceGrid(GenerationSettingsSO generationSettings) : base(generationSettings)
        {
        }

        public string Draw(Cell cell)
        {
            return ContentsOf(cell);
        }

        //This will display the distance on each cell traversed
        //by Dijkstra’s solving algorithm.
        public string ContentsOf(Cell cell)
        {
            if(Distances is not null && Distances[cell] != -1)
            {   
                //Converts to ASCII chars to get letters if the number is too big
                return Convert.ToString(Distances[cell], 16);
            }
            return " ";
        }
    }
}