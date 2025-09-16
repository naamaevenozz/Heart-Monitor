namespace DefaultNamespace.UI
{
    using UnityEngine;
    using TMPro;
    using DefaultNamespace;

    public class UIScoreDisplay : MonoBehaviour
    {
        [SerializeField] TMP_Text scoreText;

        void OnEnable()
        {
            GameEvents.ScoreChanged += UpdateScore;
            UpdateScore(0); 
        }

        void OnDisable()
        {
            GameEvents.ScoreChanged -= UpdateScore;
        }

        void UpdateScore(int newScore)
        {
            if (scoreText != null)
                scoreText.text = $"Score: {newScore}";
        }
    }

}