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



    public enum DrawMode : byte
    {
        Console,
        UIImage,
        Mesh,
    }


    public static class Enums
    {
        public static T[] ValuesOf<T>() where T : Enum
        {
            return (T[])Enum.GetValues(typeof(T));
        }
        public static string[] ValuesToString<T>() where T : Enum
        {
            T[] values = (T[])Enum.GetValues(typeof(T));
            string[] toString = new string[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                toString[i] = values[i].ToString();
            }

            return toString;
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