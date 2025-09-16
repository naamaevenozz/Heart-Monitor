using System;
using Sound;
using Utils;

namespace DefaultNamespace
{
    using UnityEngine;
    namespace Player
    {
        public class HealthSystem : MonoSingleton<HealthSystem>
        {
            public int maxHealth = 1;
            private int currentHealth;

            private void OnEnable()
            {
                GameEvents.Intro += Reset;
            }

            private void OnDisable()
            {
                GameEvents.Intro -= Reset;
            }

            private void Start()
            {
                Reset();
            }
            
            public void TakeDamage(int amount)
            {
                currentHealth -= amount;
                GameEvents.LivesChanged?.Invoke(currentHealth);

                if (currentHealth <= 0)
                {
                    GameEvents.GameOver?.Invoke();
                    SoundManager.Instance.PlaySound("LongBeep", transform);
                }
            }
            
            public void AddLives(int amount)
            {
                currentHealth += amount;
                if (currentHealth > maxHealth)
                    currentHealth = maxHealth;
                GameEvents.LivesChanged?.Invoke(currentHealth);
            }
            
            private void Reset()
            {
                currentHealth = maxHealth;
                GameEvents.LivesChanged?.Invoke(currentHealth);
            }
        }

    }
}