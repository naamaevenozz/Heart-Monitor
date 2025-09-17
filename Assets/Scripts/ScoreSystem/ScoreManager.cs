
using System;
using DefaultNamespace;
using ScoreSystem;
using Utils;

namespace ScoreSystem
{
    using UnityEngine;

    public class ScoreManager : MonoSingleton<ScoreManager>
    {
        [SerializeField] int score = 0;
        public int Score => score;
        private void OnEnable()
        {
            GameEvents.Intro += OnIntro;
            GameEvents.GameOver += OnGameOver;
        }

        private void OnDisable()
        {
            GameEvents.Intro -= OnIntro;
            GameEvents.GameOver -= OnGameOver;
        }
        
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
        
        private void OnGameOver()
        {
            HighScoreManager.Instance.TrySetNewHighScore(score);
        }
    }
}