using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using DefaultNamespace;
using DefaultNamespace.ScoreSystem;

namespace ScoreSystem
{
    public class HighScoreManager : MonoBehaviour
    {
        public static HighScoreManager Instance { get; private set; }

        private int highScore = 0;
        public int HighScore => highScore;

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
            GameEvents.GameOver += OnGameEnded;
        }

        private void OnDisable()
        {
            GameEvents.Intro -= OnIntro;
            GameEvents.GameOver -= OnGameEnded;
        }

        private void OnIntro()
        {
            LoadHighScore();
            Debug.Log($"[HighScoreManager] High score loaded: {highScore}");
        }

        private void OnGameEnded()
        {
            int finalScore = ScoreManager.Instance.Score;
            TrySetNewHighScore(finalScore);
        }

        private void LoadHighScore()
        {
            highScore = PlayerPrefs.GetInt("HighScore", 0); // 0 ברירת מחדל אם אין ערך שמור
            Debug.Log($"High Score Loaded: {highScore}");
        }

        public void TrySetNewHighScore(int finalScore)
        {
            if (finalScore > highScore)
            {
                highScore = finalScore;
                PlayerPrefs.SetInt("HighScore", highScore);
                PlayerPrefs.Save(); // חשוב לשמירה מיידית
                Debug.Log($"New High Score Saved: {highScore}");
                GameEvents.OnHighScoreChanged?.Invoke();
            }
        }
    }
}
