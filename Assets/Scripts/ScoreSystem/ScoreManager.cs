
using System;
using Utils;

namespace DefaultNamespace.ScoreSystem
{
    using UnityEngine;

    public class ScoreManager : MonoSingleton<ScoreManager>
    {
        [SerializeField] int score = 0;

        private void OnEnable()
        {
            GameEvents.Intro += OnIntro;
        }

        private void OnDisable()
        {
            GameEvents.Intro -= OnIntro;
        }

        public int Score => score;
        
        public void AddScore(int delta)
        {
            score += Mathf.Max(0, delta);
            GameEvents.ScoreChanged?.Invoke(score);
        }
        
        private void OnIntro()
        {
            score = 0;
            GameEvents.ScoreChanged?.Invoke(score);
        }
    }
}