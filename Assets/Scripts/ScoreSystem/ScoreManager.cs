
namespace DefaultNamespace.ScoreSystem
{
    using UnityEngine;

    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }
        
        [SerializeField] int score = 0;

        public int Score => score;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject); 
                return;
            }
            Instance = this;
        }
        
        public void AddScore(int delta)
        {
            score += Mathf.Max(0, delta);
            GameEvents.ScoreChanged?.Invoke(score);
        }
    }
}