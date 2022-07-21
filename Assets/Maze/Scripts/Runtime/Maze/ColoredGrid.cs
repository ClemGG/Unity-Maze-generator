using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class ColoredGrid : Grid
    {
        public Distances Distances { get; set; }
        public Cell Farthest { get; set; }
        public int Maximum { get; set; }

        public ColoredGrid(int rows, int columns) : base(rows, columns)
        {
        }

        public ColoredGrid(GenerationSettingsSO generationSettings) : base(generationSettings)
        {
        }

        public void SetDistances(Distances distances)
        {
            Distances = distances;
            (Cell, int) tuple = Distances.Max();
            Farthest = tuple.Item1;
            Maximum = tuple.Item2;
        }

        public override Color BackgroundColorFor(Cell cell)
        {
            int distance = Distances[cell];
            float intensity = (float)(Maximum - distance) / Maximum;
            float dark = Mathf.Round(255f * intensity);
            float bright = 128f + Mathf.Round(127f * intensity);

            return new(dark / 255f, bright / 255f, bright / 255f, 1f);
        }
    }
}