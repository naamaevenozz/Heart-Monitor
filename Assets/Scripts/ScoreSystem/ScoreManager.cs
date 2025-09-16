
using Utils;

namespace DefaultNamespace.ScoreSystem
{
    using UnityEngine;

    public class ScoreManager : MonoSingleton<ScoreManager>
    {
        [SerializeField] int score = 0;

        public int Score => score;
        
        public void AddScore(int delta)
        {
            score += Mathf.Max(0, delta);
            GameEvents.ScoreChanged?.Invoke(score);
        }
    }
}