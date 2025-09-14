using UnityEngine;
using System.Collections.Generic;

namespace Hidden_Points_System
{
    public class TargetPool : MonoBehaviour
    {
        public static TargetPool Instance;
        public GameObject targetPrefab;
        public int poolSize = 10;

        private Queue<Target> pool = new Queue<Target>();

        void Awake()
        {
            Instance = this;
            for (int i = 0; i < poolSize; i++)
            {
                var obj = Instantiate(targetPrefab);
                obj.SetActive(false);
                pool.Enqueue(obj.GetComponent<Target>());
            }
        }

        public Target GetTarget()
        {
            if (pool.Count > 0)
                return pool.Dequeue();
            else
                return null; 
        }

        public void ReturnToPool(Target target)
        {
            pool.Enqueue(target);
        }
    }

}