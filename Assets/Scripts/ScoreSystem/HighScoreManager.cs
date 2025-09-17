using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using DefaultNamespace;
using ScoreSystem;

namespace ScoreSystem
{
    public class HighScoreManager : MonoBehaviour
    {
        public static HighScoreManager Instance { get; private set; }

        private int _highScore = 0;
        public int HighScore => _highScore;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadHighScore();
        }

        private void OnEnable()
        {
            GameEvents.Intro += OnIntro;
        }

        private void OnDisable()
        {
            GameEvents.Intro -= OnIntro;
        }

        private void OnIntro()
        {
            LoadHighScore();
            GameEvents.OnHighScoreChanged?.Invoke();
        }

        private void LoadHighScore()
        {
            _highScore = PlayerPrefs.GetInt("HighScore", 0); 
        }

        public void TrySetNewHighScore(int finalScore)
        {
            if (finalScore > _highScore)
            {
                _highScore = finalScore;
                PlayerPrefs.SetInt("HighScore", _highScore);
                PlayerPrefs.Save(); 
                GameEvents.OnHighScoreChanged?.Invoke();
            }
        }
    }
}
