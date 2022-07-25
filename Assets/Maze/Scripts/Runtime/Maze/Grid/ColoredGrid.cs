using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class ColoredGrid : Grid, IDrawableGrid, IDrawableGrid<Color>
    {
        public ColoredGrid(int rows, int columns) : base(rows, columns)
        {
        }

        public ColoredGrid(GenerationSettingsSO generationSettings) : base(generationSettings)
        {
        }

        public Color Draw(Cell cell)
        {
            int distance = Distances[cell];
            float intensity = (float)(Maximum - distance) / Maximum;
            float dark = Mathf.Round(255f * intensity);
            float bright = 128f + Mathf.Round(127f * intensity);

            return new(dark / 255f, bright / 255f, bright / 255f, 1f);
        }
    }
}