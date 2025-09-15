using DefaultNamespace;
using UnityEngine;

namespace Hidden_Points_System
{
    using System;
using System.Collections;
using UnityEngine;
using Hidden_Points_System;

using System;
using System.Collections;
using UnityEngine;
using Hidden_Points_System;
using DefaultNamespace;

    public class TargetSpawner : MonoBehaviour
    {
        [Header("Spawn Area X")]
        public float minX = -3f;
        public float maxX = 3f;

        [Header("Default Y Range (אם WaveConfig לא מכיל גבולות)")]
        public float defaultMinY = -4f;
        public float defaultMaxY = 4f;

        private int activeTargetCount = 0;

        private void OnEnable()
        {
            GameEvents.OnWaveStarted += HandleWaveStarted;
        }

        private void OnDisable()
        {
            GameEvents.OnWaveStarted -= HandleWaveStarted;
        }

        private void HandleWaveStarted(WaveConfig config)
        {
            StartCoroutine(SpawnTargetsCoroutine(config));
        }

        private IEnumerator SpawnTargetsCoroutine(WaveConfig config)
        {
            activeTargetCount = config.targetAmount;

            for (int i = 0; i < config.targetAmount; i++)
            {
                SpawnSingleTarget(config.lifetime, config);
                yield return new WaitForSeconds(config.spawnDelay);
            }
        }

        private void SpawnSingleTarget(float lifeTime, WaveConfig config)
        {
            Target target = TargetPool.Instance.GetTarget();

            if (target == null)
            {
                Debug.LogWarning("TargetPool returned null!");
                return;
            }

            target.OnTargetReturned -= HandleTargetReturned;
            target.OnTargetReturned += HandleTargetReturned;

            float minY = defaultMinY;
            float maxY = defaultMaxY;
            
            Vector2 spawnPos = new Vector2(
                UnityEngine.Random.Range(minX, maxX),
                UnityEngine.Random.Range(minY, maxY)
            );

            target.Activate(lifeTime, spawnPos);
        }

        private void HandleTargetReturned()
        {
            activeTargetCount--;

            if (activeTargetCount <= 0)
            {
                Debug.Log("wave ended, all targets returned");
                GameEvents.OnWaveEnded?.Invoke();
            }
        }
    }


}