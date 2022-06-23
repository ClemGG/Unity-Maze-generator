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