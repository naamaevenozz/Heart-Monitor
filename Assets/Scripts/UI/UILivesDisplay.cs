namespace DefaultNamespace.UI
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections.Generic;
    using DefaultNamespace;

    public class UILivesDisplay : MonoBehaviour
    {
        [SerializeField] List<Image> heartIcons = new();

        void OnEnable()
        {
            GameEvents.LivesChanged += UpdateHearts;
        }

        void OnDisable()
        {
            GameEvents.LivesChanged -= UpdateHearts;
        }

        void UpdateHearts(int livesLeft)
        {
            for (int i = 0; i < heartIcons.Count; i++)
                heartIcons[i].enabled = i < livesLeft;
        }
    }

}