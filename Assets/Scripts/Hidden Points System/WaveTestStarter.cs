namespace Hidden_Points_System
{
    using UnityEngine;
    using DefaultNamespace;
    using Hidden_Points_System;

    public class WaveTestStarter : MonoBehaviour
    {
        public WaveConfig testWave;

        void Start()
        {
            Invoke(nameof(StartTestWave), 1f); 
        }

        void StartTestWave()
        {
            if (testWave != null)
            {
                Debug.Log("Wave test starting WaveTestStarter");
                GameEvents.OnWaveStarted?.Invoke(testWave);
            }
            else
            {
                Debug.LogError("Wave config is not assigned in WaveTestStarter");
            }
        }
    }

}