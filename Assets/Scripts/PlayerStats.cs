using DefaultNamespace;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [SerializeField] int lives = 3;
    [SerializeField] int score = 0;

    public int Lives => lives;
    public int Score => score;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // מונע כפילויות
            return;
        }

        Instance = this;

        // אם צריך שהוא ישרוד טעינת סצנה:
        // DontDestroyOnLoad(gameObject);
    }

    public void AddScore(int delta)
    {
        score += Mathf.Max(0, delta);
        GameEvents.ScoreChanged?.Invoke(score);
    }

    public void LoseLife(int delta = 1)
    {
        lives = Mathf.Max(0, lives - Mathf.Max(0, delta));
        GameEvents.LivesChanged?.Invoke(lives);
        if (lives <= 0)
        {
            GameEvents.GameOver?.Invoke();
        }
    }
}