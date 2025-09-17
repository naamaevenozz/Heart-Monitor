using System;
using DefaultNamespace;
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
         public int waveIndex ;

        
         void Start()
        {
            waveIndex  = 0;
            // Should be called from UI event
            //OnGameStart();
            GameEvents.Intro?.Invoke();
        }

        private void OnEnable()
        {
            GameEvents.OnWaveEnded += NextWave;
            // GameEvents.GameOver += EndGame;
            GameEvents.GameStarted += OnGameStart;
        }
        private void OnDisable()
        {
            GameEvents.OnWaveEnded -= NextWave;
            // GameEvents.GameOver -= EndGame;
            GameEvents.GameStarted -= OnGameStart;
        }

        private void EndGame()
        { 
            GameEvents.OnWaveStarted?.Invoke(waveConfigs[10]);
            waveIndex = 10;
        }

        private void NextWave()
        {
            Debug.Log($"Next wave at {waveIndex}");
            StartWave();
        }

        void OnGameStart()
        {
            StartWave();
        }
        void StartWave()
        {
            WaveConfig waveConfig = waveConfigs[waveIndex];
            GameEvents.OnWaveStarted?.Invoke(waveConfig);
            waveIndex++;
        }

    }

}