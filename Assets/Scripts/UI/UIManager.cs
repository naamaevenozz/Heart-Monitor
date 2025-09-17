using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;
namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject introPanel;
        [SerializeField] private GameObject gamePanel;
        [SerializeField] private GameObject endPanel;

        private void OnEnable()
        {
            GameEvents.Intro += ShowIntro;
            GameEvents.GameStarted += ShowGame;
            GameEvents.GameOver += ShowEnd;
        }

        private void OnDisable()
        {
            GameEvents.Intro -= ShowIntro;
            GameEvents.GameStarted -= ShowGame;
            GameEvents.GameOver -= ShowEnd;
        }

        private void ShowIntro()
        {
            introPanel.SetActive(true);
            gamePanel.SetActive(false);
            endPanel.SetActive(false);
        }

        private void ShowGame()
        {
            introPanel.SetActive(false);
            gamePanel.SetActive(true);
            endPanel.SetActive(false);
        }

        private void ShowEnd()
        {
            introPanel.SetActive(false);
            gamePanel.SetActive(false);
            endPanel.SetActive(true);
        }
    }

}