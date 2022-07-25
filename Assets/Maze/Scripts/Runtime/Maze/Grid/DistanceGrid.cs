using System;
using System.Text;

namespace Project.Procedural.MazeGeneration
{
    public class DistanceGrid : Grid, IDrawableGrid, IDrawableGrid<string>
    {
        public DistanceGrid(int rows, int columns) : base(rows, columns)
        {
        }

        public DistanceGrid(GenerationSettingsSO generationSettings) : base(generationSettings)
        {
        }


        //This will display the distance on each cell traversed
        //by Dijkstra’s solving algorithm.
        public string Draw(Cell cell)
        {
            if(Distances is not null && Distances[cell] != -1)
            {   
                //Converts to ASCII chars to get letters if the number is too big
                return Convert.ToString(Distances[cell], 16);
            }
            return " ";
        }


        public override string ToString()
        {
            //6 refers to the numbers of colums per cell
            //3 refers to the numbers of rows per cell
            StringBuilder output = new(6 * 3 * Rows * Columns);
            output.Append("+");
            output.Insert(1, "---+", Columns);
            output.Append("\n");

            foreach (Cell[] row in EachRow())
            {
                var top = "|";
                var bottom = "+";

                for (int i = 0; i < row.Length; i++)
                {
                    Cell cell = row[i];
                    if (cell is null)
                    {
                        cell = new(-1, -1);
                    }

                    var body = $" {Draw(cell)} ";   //3 spaces for the Cell's body
                    var eastBoundary = cell.IsLinked(cell.East) ? " " : "|";
                    top = string.Concat(top, body, eastBoundary);

                    //3 spaces below, too
                    var southBoundary = cell.IsLinked(cell.South) ? "   " : "---";
                    var corner = "+";
                    bottom = string.Concat(bottom, southBoundary, corner);

                }
                output.Append($"{top}\n");
                output.Append($"{bottom}\n");
            }

            return output.ToString();
        }
    }
}