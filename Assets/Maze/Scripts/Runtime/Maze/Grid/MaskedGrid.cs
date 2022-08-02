using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public class MaskedGrid : Grid, IDrawableGrid, IDrawableGrid<Color>, IMaskable
    {
        public Mask Mask { get; set; }

        public MaskedGrid(int rows, int cols) : base(rows, cols)
        {

        }
        public MaskedGrid(GenerationSettingsSO generationSettings) : base(generationSettings)
        {

        }

        public MaskedGrid(Mask mask) : base(mask.Rows, mask.Columns)
        {
            Mask = mask;
            PrepareGrid();
            ConfigureCells();
        }


        //We add "new" to these methods because we don't want to call the base
        //in order to use the mask
        public new void PrepareGrid()
        {
            _grid = new Cell[Rows][];
            for (int i = 0; i < Rows; i++)
            {
                _grid[i] = new Cell[Columns];
                for (int j = 0; j < Columns; j++)
                {
                    if (Mask[i, j])
                    {
                        _grid[i][j] = new(i, j);
                    }
                    else
                    {
                        _grid[i][j] = null;
                    }
                }
            }
        }


        public new void ConfigureCells()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Cell c = _grid[i][j];

                    if (c is not null)
                    {
                        c.North = this[i - 1, j];
                        c.South = this[i + 1, j];
                        c.West = this[i, j - 1];
                        c.East = this[i, j + 1];
                    }
                }
            }
        }

        public override Cell RandomCell()
        {
            (int row, int col) = Mask.RandomLocation();
            return this[row, col];
        }

        public override int Size() => Mask.ActiveCellsCount;


        //We also could have directly inherited from ColoredGrid,
        //but it's better to keep the implementations separate.
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