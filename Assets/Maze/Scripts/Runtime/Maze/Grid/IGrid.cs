using System.Collections.Generic;

namespace Project.Procedural.MazeGeneration
{
    public interface IGrid
    {
        int Rows { get; }
        int Columns { get; }
        float BraidRate { get; }

        int Size();
        Cell RandomCell();
        Cell this[int row, int column] { get; }

        IEnumerable<Cell[]> EachRow();
        IEnumerable<Cell> EachCell();

        List<Cell> GetDeadends();
        void Braid();
        void LinkAll();
        void UnlinkAll();
    }

    //Used to show the contents of the Cell to avoid type mismatchs (string, Color, etc.)
    public interface IDrawableGrid<out T>
    {
        //Implemented in derived classes in case each Grid wants to display
        //the info differently (like colored strings or normal ones)
        public T Draw(Cell cell);
    }


    public interface IDistanceGrid : IDrawableGrid<string>
    {
        Distances Distances { get; set; }

        //Describes what to use to represent the Cell in the display classes
        string ContentsOf(Cell cell)
        {
            return " ";
        }
    }
}