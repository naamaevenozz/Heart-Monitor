using UnityEngine;
using System.Collections.Generic;

namespace Hidden_Points_System
{
    public class TargetPool : MonoBehaviour
    {
        public static TargetPool Instance;

        [Header("Pool Settings")]
        public GameObject targetPrefab;
        public int poolSize = 10;

        private readonly Queue<Target> pool = new Queue<Target>();

        void Awake()
        {
            Instance = this;

            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(targetPrefab);
                obj.SetActive(false);

                Target target = obj.GetComponent<Target>();
                if (target != null)
                    pool.Enqueue(target);
                else
                    Debug.LogError("Target prefab missing Target component!");
            }
        }

        public Target GetTarget()
        {
            if (pool.Count > 0)
            {
                return pool.Dequeue();
            }
            else
            {
                Debug.LogWarning("Target pool is empty! Instantiating a new one.");

                GameObject obj = Instantiate(targetPrefab);
                obj.SetActive(false);
                Target newTarget = obj.GetComponent<Target>();
                return newTarget;
            }
        }

        public void ReturnToPool(Target target)
        {
            target.ResetTarget();
            target.gameObject.SetActive(false);
            pool.Enqueue(target);
        }
    }
}