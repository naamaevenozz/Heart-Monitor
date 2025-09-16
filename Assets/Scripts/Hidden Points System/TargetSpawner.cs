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

        private readonly System.Collections.Generic.List<Target> spawnedTargets = new();

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
            StartCoroutine(SpawnWave(config));
        }

        private IEnumerator SpawnWave(WaveConfig config)
        {
            spawnedTargets.Clear();

            for (int i = 0; i < config.targetAmount; i++)
            {
                SpawnSingleTarget(config.lifetime, config);
                yield return new WaitForSeconds(config.spawnDelay);
            }

            // אחרי שכל המטרות שוגרו — נתחיל לעקוב מתי הן נעלמות
            yield return StartCoroutine(WaitForAllTargetsToDisappear());
        }

        private void SpawnSingleTarget(float lifeTime, WaveConfig config)
        {
            Target target = TargetPool.Instance.GetTarget();

            if (target == null)
            {
                Debug.LogWarning("TargetPool returned null!");
                return;
            }

            float minY = defaultMinY;
            float maxY = defaultMaxY;

            Vector2 spawnPos = new Vector2(
                UnityEngine.Random.Range(screenLeftX, screenRightX),
                UnityEngine.Random.Range(minY, maxY)
            );

            target.Activate(lifeTime, spawnPos);
            spawnedTargets.Add(target);
        }

        private IEnumerator WaitForAllTargetsToDisappear()
        {
            while (true)
            {
                // כל עוד יש מטרות פעילות
                bool anyStillActive = false;

                foreach (var target in spawnedTargets)
                {
                    if (target != null && target.gameObject.activeSelf)
                    {
                        anyStillActive = true;
                        break;
                    }
                }

                if (!anyStillActive)
                    break;

                yield return null;
            }

            Debug.Log("Wave ended! All targets are gone.");
            GameEvents.OnWaveEnded?.Invoke();
        }
    }
}
