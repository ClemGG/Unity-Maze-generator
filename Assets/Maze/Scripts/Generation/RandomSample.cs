using System.Collections.Generic;
using UnityEngine;

namespace Project.Procedural.MazeGeneration
{
    public static class RandomSample
    {
        public static T Sample<T>(this List<T> list, int start = 0)
        {
            if(list.Count == 0)
            {
                return default;
            }
            int random = Random.Range(start, list.Count);
            return list[random];
        }

        public static T Sample<T>(this T[] array, int start = 0)
        {
            if (array.Length == 0)
            {
                return default;
            }
            int random = Random.Range(start, array.Length);
            return array[random];
        }

        public static int Sample(this int length, int start = 0)
        {
            if (length == 0)
            {
                Debug.LogError("Sample Error : Length is 0. Defaulted to 0 to avoid range error.");
                return 0;
            }
            return Random.Range(start, length);
        }

        public static float Sample(this float length, float start = 0f)
        {
            if (Mathf.Approximately(length, 0f))
            {
                Debug.LogError("Sample Error : Length is 0f. Defaulted to 0f to avoid range error.");
                return 0f;
            }
            return Random.Range(start, length);
        }
    }
}