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

    public interface IDistanceGrid
    {
        Distances Distances { get; set; }
        Cell Farthest { get; set; }
        int Maximum { get; set; }
    }

    public interface IDrawableGrid : IGrid, IDistanceGrid
    {
        object Draw(Cell cell);
        void SetDistances(Distances distances)
        {
            Distances = distances;
            (Cell, int) tuple = Distances.Max();
            Farthest = tuple.Item1;
            Maximum = tuple.Item2;
        }
    }

    //Used to show the contents of the Cell to avoid type mismatchs (string, Color, etc.)
    public interface IDrawableGrid<out T> : IDrawableGrid
    {
        //Describes what to use to represent the Cell in the display classes
        //(like colored Tiles or chars)
        object IDrawableGrid.Draw(Cell cell) => Draw(cell);
        T Draw(Cell cell);
    }
}