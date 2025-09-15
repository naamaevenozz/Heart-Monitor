using DefaultNamespace;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] int lives = 3;
    [SerializeField] int score = 0;

    public int Lives => lives;
    public int Score => score;

    public void AddScore(int delta)
    {
        score += Mathf.Max(0, delta);
        GameEvents.ScoreChanged?.Invoke(score);
    }

    public void LoseLife(int delta = 1)
    {
        lives = Mathf.Max(0, lives - Mathf.Max(0, delta));
        GameEvents.LivesChanged?.Invoke(lives);
        GameEvents.PlayerLostLife?.Invoke();
    }
}