using UnityEngine;
using TMPro; 
using DefaultNamespace.ScoreSystem;
namespace UI
{
    public class HighScoreUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text highScoreText;
        [SerializeField] private TMP_Text lastScoreText; 

        private void OnEnable()
        {
            UpdateTexts();
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