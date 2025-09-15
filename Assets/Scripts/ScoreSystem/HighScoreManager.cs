using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace DefaultNamespace.ScoreSystem
{
    public class HighScoreManager : MonoBehaviour
    {
        public static HighScoreManager Instance { get; private set; }

        private string highScoreFilePath;
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

            highScoreFilePath = Path.Combine(Application.persistentDataPath, "highscore.txt");
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
            if (File.Exists(highScoreFilePath))
            {
                string content = File.ReadAllText(highScoreFilePath);
                if (int.TryParse(content, out int savedScore))
                {
                    highScore = savedScore;
                }
                else
                {
                    Debug.LogWarning("Failed to parse high score from file.");
                    highScore = 0;
                }
            }
            else
            {
                highScore = 0;
            }

            Debug.Log($"High Score Loaded: {highScore}");
        }

        public void TrySetNewHighScore(int finalScore)
        {
            if (finalScore > highScore)
            {
                highScore = finalScore;
                File.WriteAllText(highScoreFilePath, highScore.ToString());
                Debug.Log($"New High Score Saved: {highScore}");
            }
        }
    }
}
