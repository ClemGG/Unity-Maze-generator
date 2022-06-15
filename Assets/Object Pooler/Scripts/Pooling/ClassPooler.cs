using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

//Used to instantiate any type of class, including those not deriving from MonoBehaviour or ScriptableObject

namespace Project.Pool
{



    [System.Serializable]
    public class ClassPooler<TBase> where TBase : class
    {

        [SerializeField] private Dictionary<string, ObjectPool<TBase>> _poolDictionary { get; set; }


        #region Constructors

        public ClassPooler(params Pool<TBase>[] newPools)
        {
            _poolDictionary = new Dictionary<string, ObjectPool<TBase>>(newPools.Length);
            AddPools(newPools);
        }


        private ObjectPool<TBase> CreatePool(int defaultCapacity, Func<TBase> createFunc)
        {
            ObjectPool<TBase> newPool = new ObjectPool<TBase>(
                createFunc: () => createFunc.Invoke(),
                actionOnGet: (obj) => Dequeue(obj),
                actionOnRelease: (obj) => Enqueue(obj),
                collectionCheck: false,
                defaultCapacity: defaultCapacity
                );

            return newPool;
        }


        public void AddPools(params Pool<TBase>[] newPools)
        {
            for (int i = 0; i < newPools.Length; i++)
            {
                _poolDictionary.Add(newPools[i].Key, CreatePool(newPools[i].DefaultCpapcity, newPools[i].CreateFunc));
            }

        }


        public void RemovePools(params string[] keys)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (_poolDictionary.ContainsKey(keys[i]))
                {
                    _poolDictionary[keys[i]].Dispose();
                    _poolDictionary.Remove(keys[i]);
                }
            }
        }

        #endregion





        #region Pooling

        private static void Dequeue(TBase obj)
        {
            //(IDequeued)obj causes an InvalidCastException if obj does not derive from the interface
            if (obj is IDequeued pooledObj)
            {
                pooledObj.OnDequeued();
            }
            else if(obj is GameObject go)
            {
                IDequeued[] ids = go.GetComponents<IDequeued>();
                for (int i = 0; i < ids.Length; i++)
                {
                    ids[i].OnDequeued();
                }
            }
        }
        private static void Enqueue(TBase obj)
        {
            //(IEnqueued)obj causes an InvalidCastException if obj does not derive from the interface
            if (obj is IEnqueued pooledObj)
            {
                pooledObj.OnEnqueued();
            }
            else if (obj is GameObject go)
            {
                IEnqueued[] ids = go.GetComponents<IEnqueued>();
                for (int i = 0; i < ids.Length; i++)
                {
                    ids[i].OnEnqueued();
                }
            }
            
        }


        public PooledObject<TChild> UsingFromPool<TChild>(string key = null) where TChild : class, TBase
        {

            if (key is null) key = typeof(TChild).Name;

            if (!_poolDictionary.ContainsKey(key))
            {
                Debug.LogError($"Pooler Error : The key '{key}' does not exist.");
                return default;
            }

            ObjectPool<TChild> pool = _poolDictionary[key] as ObjectPool<TChild>;

            return new PooledObject<TChild>(pool.Get(), pool);
        }

        public TChild GetFromPool<TChild>(string key = null) where TChild : TBase
        {

            if (key is null) key = typeof(TChild).Name;

            if (!_poolDictionary.ContainsKey(key))
            {
                Debug.LogError($"Pooler Error : The key '{key}' does not exist.");
                return default;
            }

            TChild obj = (TChild)_poolDictionary[key].Get();


            return obj;
        }

        public void ReturnToPool(TBase obj, string key = null)
        {
            if(key is null) key = obj.GetType().Name;

            if (!_poolDictionary.ContainsKey(key))
            {
                Debug.LogError($"Pooler Error : The key '{key}' does not exist.");
                return;
            }

            _poolDictionary[key].Release(obj);
        }

        #endregion

    }
}