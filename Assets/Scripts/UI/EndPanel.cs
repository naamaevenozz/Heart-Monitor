using UnityEngine;
using UnityEngine.EventSystems;
using DefaultNamespace;

namespace UI
{
    public class EndPanel : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            GameEvents.Intro?.Invoke();
        }
    }
}