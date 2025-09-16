using DefaultNamespace;
using System.Collections;
using Sound;
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
            GameEvents.GameOver += Reset;
        }

        private void OnDisable()
        {
            GameEvents.OnWaveStarted -= HandleWaveStarted;
            GameEvents.GameOver -= Reset;
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
            defaultMinY = config.minBound;
            defaultMaxY = config.maxBound;

            for (int i = 0; i < config.targetAmount; i++)
            {
                SpawnSingleTarget(config.lifetime, config);
                SoundManager.Instance.PlaySound("ShortBeep", transform);
                yield return new WaitForSeconds(config.spawnDelay);
            }

            yield return StartCoroutine(WaitForAllTargetsToDisappear());
        }

        private void SpawnSingleTarget(float lifeTime, WaveConfig config)
        {
            Target target = TargetPool.Instance.Get();

            if (target == null)
            {
                Debug.LogWarning("TargetPool returned null!");
                return;
            }

            
            Vector2 spawnPos = new Vector2(
                UnityEngine.Random.Range(screenLeftX, screenRightX),
                UnityEngine.Random.Range(defaultMinY, defaultMaxY)
            );

            target.Activate(lifeTime, spawnPos);
            spawnedTargets.Add(target);
        }

        private IEnumerator WaitForAllTargetsToDisappear()
        {
            while (true)
            {
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

        private void Reset()
        {
            StopAllCoroutines();
            foreach (var target in spawnedTargets)
            {
                if (target != null && target.gameObject.activeSelf)
                {
                    target.gameObject.SetActive(false);
                    TargetPool.Instance.Return(target);
                }
            }
            spawnedTargets.Clear();
        }
    }
}
