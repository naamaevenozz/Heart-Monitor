namespace DefaultNamespace
{
    using UnityEngine;
    namespace Player
    {
        public class HealthSystem : MonoBehaviour
        {
            public static HealthSystem Instance { get; private set; }
            public int maxHealth = 1;
            private int currentHealth;
            
            private void Awake()
            {
                if (Instance != null && Instance != this)
                {
                    Destroy(gameObject); 
                    return;
                }
                Instance = this;
            }
            private void Start()
            {
                currentHealth = maxHealth;
                GameEvents.LivesChanged?.Invoke(currentHealth);
            }

            public void TakeDamage(int amount)
            {
                currentHealth -= amount;
                GameEvents.LivesChanged?.Invoke(currentHealth);

                if (currentHealth <= 0)
                {
                    GameEvents.GameOver?.Invoke();
                }
            }
            
            public void AddLives(int amount)
            {
                currentHealth += amount;
                if (currentHealth > maxHealth)
                    currentHealth = maxHealth;
                GameEvents.LivesChanged?.Invoke(currentHealth);
            }
        }

    }
}