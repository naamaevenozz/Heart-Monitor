using DefaultNamespace;
using UnityEngine;
using TMPro; 
using ScoreSystem;

namespace UI
{
    public class HighScoreUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text highScoreText;
        [SerializeField] private TMP_Text lastScoreText; 

        private void OnEnable()
        {
            UpdateTexts();
            GameEvents.OnHighScoreChanged+=UpdateTexts;
            GameEvents.GameOver+=UpdateTexts;
        }
        private void OnDisable()
        {
            GameEvents.OnHighScoreChanged-=UpdateTexts;
            GameEvents.GameOver-=UpdateTexts;
        }

        private void UpdateTexts()
        {
            if (highScoreText != null && HighScoreManager.Instance != null)
                highScoreText.text = $"Best Score: {HighScoreManager.Instance.HighScore}";

            if (lastScoreText != null && ScoreManager.Instance != null)
                lastScoreText.text = $"Your Score: {ScoreManager.Instance.Score}";
        }
    }

}