using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Utilities
{
    public class MonoPool<T> : MonoSingleton<MonoPool<T>> where T : MonoBehaviour, IPoolable
    {
        [SerializeField] private int initialPoolSize = 25;
        [SerializeField] private T prefab;
        
        private Stack<T> _pool;
    
        private void Awake()
        {
            _pool = new Stack<T>();
            GrowPool();
        }
    
        public T Get()
        {
            if (_pool.Count == 0)
                GrowPool();
            var pooledObject = _pool.Pop();           
            pooledObject.gameObject.SetActive(true);
            return pooledObject;
        }
    
        public void Return(T pooledObject)
        {
            pooledObject.Reset();
            pooledObject.gameObject.SetActive(false);
            _pool.Push(pooledObject);
        }

        private void GrowPool()
        {
            for (var i = 0; i < initialPoolSize; i++)
            {
                var pooledObject = Instantiate(prefab, transform, true);
                pooledObject.Reset();
                pooledObject.gameObject.SetActive(false);
                _pool.Push(pooledObject);
            }
        }
    }
}