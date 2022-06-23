namespace Project.Procedural.MazeGeneration
{
    public class MaskedGrid : Grid
    {
        public Mask Mask { get; private set; }

        public MaskedGrid(int rows, int cols) : base(rows, cols)
        {

        }

        public void SetMask(Mask m)
        {
            Mask = m;
            PrepareGrid();
            ConfigureCells();
        }

        protected new void PrepareGrid()
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


        protected new void ConfigureCells()
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
    }
}