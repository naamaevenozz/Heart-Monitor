using UnityEngine;
using UnityEngine.UI;
using DefaultNamespace;

namespace UI
{
    public class IntroPanel : MonoBehaviour
    {
        public Button startButton;
        public Button tutorialButton;

        private void Start()
        {
            startButton.onClick.AddListener(StartGame);
            tutorialButton.onClick.AddListener(StartTutorial);
        }

        private void StartGame() => GameEvents.GameStarted?.Invoke();
        private void StartTutorial() => GameEvents.Tutorial?.Invoke();
    }
}