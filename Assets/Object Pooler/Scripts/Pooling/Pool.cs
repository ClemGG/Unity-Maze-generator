using System;

namespace Project.Pool
{
    public class Pool<T> where T : class
    {
        public string Key { get; }
        public int DefaultCpapcity { get; }
        public Func<T> CreateFunc { get; }

        public Pool(string key, int defaultCapacity, Func<T> createFunc)
        {
            Key = key;
            DefaultCpapcity = defaultCapacity;
            CreateFunc = createFunc;
        }
    }
}