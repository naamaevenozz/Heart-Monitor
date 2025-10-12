using System;
using System.Collections;
using DefaultNamespace;
using Sound;
using UnityEditor.Analytics;
using UnityEngine;

namespace Hidden_Points_System
{

    public class GameManager : MonoBehaviour
    {
        [SerializeField] float xMin ;
        [SerializeField] float xMax ;
        [SerializeField] float yMin ;
        [SerializeField] float yMax ;
         public float baseLifeTime ;
         [SerializeField] WaveConfig [] waveConfigs;
         [SerializeField] private WaveConfig finalWave;
         [SerializeField] private WaveConfig tutorialWave;
         public int waveIndex ;
         private bool _isTutorialWave = false;

        
         void Start()
        {
            waveIndex  = 0;
            GameEvents.Intro?.Invoke();
        }

        private void OnEnable()
        {
            GameEvents.OnWaveEnded += NextWave;
            GameEvents.GameStarted += OnGameStart;
            GameEvents.Tutorial += OnTutorial;
        }

        private void OnDisable()
        {
            GameEvents.OnWaveEnded -= NextWave;
            GameEvents.GameStarted -= OnGameStart;
            GameEvents.Tutorial -= OnTutorial;
        }

        private void NextWave()
        {
            if (_isTutorialWave)
            {
                _isTutorialWave = false;
                waveIndex = 0;
                GameEvents.OnTutorialEnd?.Invoke();
                StartCoroutine(BackToIntro());
            }
            else
            {
                Debug.Log($"Next wave at {waveIndex}");
                StartWave();
            }
        }

        void OnGameStart()
        {
            waveIndex = 0;
            StartWave();
        }
        void StartWave()
        {
            if (waveIndex == 29)
            {
                waveIndex = 0;
            }
            WaveConfig waveConfig = waveConfigs[waveIndex];
            GameEvents.OnWaveStarted?.Invoke(waveConfig);
            waveIndex++;
        }

        private void OnTutorial()
        {
            _isTutorialWave = true;
            GameEvents.OnWaveStarted?.Invoke(tutorialWave);
        }
        
        private IEnumerator BackToIntro()
        {
            yield return new WaitForSeconds(2f);
            GameEvents.Intro?.Invoke();
        }
    }

}