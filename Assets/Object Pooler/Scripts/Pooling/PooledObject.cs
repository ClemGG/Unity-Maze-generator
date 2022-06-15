using System;
using UnityEngine.Pool;

namespace Project.Pool
{

    /// <summary>
    /// A Pooled object wraps a reference to an instance that will be returned to the pool when the Pooled object is disposed.
    /// The purpose is to automate the return of references so that they do not need to be returned manually.
    /// A PooledObject can be used like so:
    /// <code>
    /// 
    /// using(myPool.Get(out MyClass myInstance)) // When leaving the scope myInstance will be returned to the pool.
    /// {
    ///     // Do something with myInstance
    /// }
    /// </code>
    /// </summary>
    public struct PooledObject<T> : IDisposable where T : class
    {
        readonly T m_ToReturn;
        readonly ObjectPool<T> m_Pool;

        internal PooledObject(T value, ObjectPool<T> pool)
        {
            m_ToReturn = value;
            m_Pool = pool;
        }

        void IDisposable.Dispose() => m_Pool.Release(m_ToReturn);
    }
}