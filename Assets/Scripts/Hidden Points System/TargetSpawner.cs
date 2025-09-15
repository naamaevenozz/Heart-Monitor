using DefaultNamespace;
using System.Collections;
using UnityEngine;

namespace Hidden_Points_System
{
    public class TargetSpawner : MonoBehaviour
    {
        [Header("Default Y Range (if wave config doesn't specify)")]
        public float defaultMinY = -3f;
        public float defaultMaxY = 3f;

        private float screenLeftX;
        private float screenRightX;

        private int activeTargetCount = 0;

        private void OnEnable()
        {
            GameEvents.OnWaveStarted += HandleWaveStarted;
        }

        private void OnDisable()
        {
            GameEvents.OnWaveStarted -= HandleWaveStarted;
        }

        private void Start()
        {
            Vector3 left = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector3 right = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));

            screenLeftX = left.x;
            screenRightX = right.x;
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
                UnityEngine.Random.Range(screenLeftX, screenRightX),
                UnityEngine.Random.Range(minY, maxY)
            );

            target.Activate(lifeTime, spawnPos);
        }

        private void HandleTargetReturned()
        {
            activeTargetCount--;

            if (activeTargetCount <= 0)
            {
                Debug.Log("Wave ended, all targets returned.");
                GameEvents.OnWaveEnded?.Invoke();
            }
        }
    }
}