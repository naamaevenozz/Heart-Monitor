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
        [SerializeField] private GameObject tutorialPanel;
        [SerializeField] private GameObject tutorialTxt;

        private void OnEnable()
        {
            GameEvents.Intro += ShowIntro;
            GameEvents.GameStarted += ShowGame;
            GameEvents.GameOver += ShowEnd;
            GameEvents.Tutorial += ShowTutorial;
            GameEvents.OnTutorialEnd += TutorialFeedback;
        }

        private void OnDisable()
        {
            GameEvents.Intro -= ShowIntro;
            GameEvents.GameStarted -= ShowGame;
            GameEvents.GameOver -= ShowEnd;
            GameEvents.Tutorial -= ShowTutorial;
            GameEvents.OnTutorialEnd -= TutorialFeedback;
        }

        private void ShowIntro()
        {
            introPanel.SetActive(true);
            gamePanel.SetActive(false);
            endPanel.SetActive(false);
            tutorialPanel.SetActive(false);
            tutorialTxt.SetActive(false);
        }

        private void ShowGame()
        {
            introPanel.SetActive(false);
            gamePanel.SetActive(true);
            endPanel.SetActive(false);
            tutorialPanel.SetActive(false);
            tutorialTxt.SetActive(false);
        }

        private void ShowEnd()
        {
            introPanel.SetActive(false);
            gamePanel.SetActive(false);
            endPanel.SetActive(true);
            tutorialPanel.SetActive(false);
            tutorialTxt.SetActive(false);
        }
        
        private void ShowTutorial()
        {
            introPanel.SetActive(false);
            gamePanel.SetActive(false);
            endPanel.SetActive(false);
            tutorialPanel.SetActive(true);
            tutorialTxt.SetActive(false);
        }
        
        private void TutorialFeedback()
        {
            tutorialTxt.SetActive(true);
        }
    }

}