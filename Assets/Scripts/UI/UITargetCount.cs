using System;
using DefaultNamespace;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UITargetCount : MonoBehaviour
    {
        [SerializeField] private TMP_Text targetCountText;
        private int count = 0;

        private void OnEnable()
        {
            GameEvents.OnTargetCountChanged += UpdateTargetCount;
            count = 0;
            UpdateTargetCount(0); 
        }
        private void OnDisable()
        {
            GameEvents.OnTargetCountChanged -= UpdateTargetCount;
        }

        private void UpdateTargetCount(int arg)
        {
            count += arg;
            if (targetCountText != null)
                targetCountText.text = $"Disruptions: {count}";
        }
    }
}