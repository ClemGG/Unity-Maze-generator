using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class Mask
    {

        public int Rows { get; }
        public int Columns { get; }
        public bool[][] Bits { get; }

        public int ActiveCellsCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        if (Bits[i][j] is true)
                        {
                            count++;
                        }
                    }
                }

                return count;
            }
        }


        public static Mask FromText(string fileName)
        {
            List<string> lines = File.ReadAllLines($"{Application.dataPath}/Maze/StreamingAssets/Masks/{fileName}.txt").ToList();
            //Removes any whitespace or special characters we don't want
            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                {
                    lines.Remove(line);
                }
                line.Trim();
            }

            int rows = lines.Count;
            int cols = lines[0].Length;
            Mask m = new(rows, cols);

            //Masks all cells with an X on them
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (lines[i][j] is 'X')
                    {
                        m[i, j] = false;
                    }
                    else
                    {
                        m[i, j] = true;
                    }
                }
            }

            return m;
        }
        public static Mask FromImgFile(Texture2D imageAsset, string extension)
        {
            byte[] texData = File.ReadAllBytes($"{Application.dataPath}/Maze/StreamingAssets/Masks/{imageAsset.name}{extension}");

            Texture2D tex = new(2, 2);
            tex.LoadImage(texData);

            int rows = tex.height;
            int cols = tex.width;
            Mask m = new(rows, cols);

            //Masks all cells representing a black pixel
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Color pixel = tex.GetPixel(j, rows-1-i);
                    if (pixel == Color.black)
                    {
                        m[i, j] = false;
                    }
                    else
                    {
                        m[i, j] = true;
                    }
                }
            }

            return m;
        }


        
        public Mask(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Bits = new bool[rows][];
            for (int i = 0; i < rows; i++)
            {
                Bits[i] = new bool[columns];
                for (int j = 0; j < columns; j++)
                {
                    Bits[i][j] = true;
                }
            }
        }


        public bool this[int row, int column]
        {
            get
            {
                if (row >= 0 && row < Rows && column >= 0 && column < Columns)
                {
                    return Bits[row][column];
                }

                return false;
            }
            set
            {
                Bits[row][column] = value;
            }
        }

        //Returns a random Cell among all the active cells
        public (int, int) RandomLocation()
        {
            do
            {
                int randX = Rows.Sample();
                int randY = Rows.Sample();

                if (Bits[randX][randY])
                {
                    return (randX, randY);
                }
            } while (true);

        }
    }
}