using System;

namespace Project.Procedural.MazeGeneration
{
    public enum GenerationType : byte
    {
        OneRoom,
        BinaryTree,
        Sidewinder,
        AldousBroder,
        Wilson,
        Houston,
        HuntAndKill,
        RecursiveBacktracker,
        RandomizedKruskal,
        SimplifiedPrim,
        TruePrim,
        GrowingTree,
        Eller,
        RecursiveDivision,
    }



    public enum DisplayMode : byte
    {
        Print,
        UIImage,
        Mesh,
    }

    public enum MazeType : byte
    {
        Orthogonal, //Gamma
    }

    public static class Enums
    {
        public static T[] ValuesOf<T>() where T : Enum
        {
            return (T[])Enum.GetValues(typeof(T));
        }

        public static int LengthOf<T>() where T : Enum
        {
            return ValuesOf<T>().Length;
        }

        public static void ForEach<T>(Action<T> action) where T : Enum
        {
            foreach (T value in ValuesOf<T>())
            {
                action.Invoke(value);
            }
        }
    }
}