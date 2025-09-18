using UnityEngine;
using UnityEngine.EventSystems;
using DefaultNamespace;

namespace UI
{

    public class ClickAnywherePanel : MonoBehaviour, IPointerClickHandler
    {
        public enum PanelType { Intro, End }
        public PanelType type;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (type == PanelType.Intro)
            {
                Vector2 clickPosition = eventData.position; 
                
                GameEvents.GameStarted?.Invoke();
            }
            else if (type == PanelType.End)
                GameEvents.Intro?.Invoke();
        }
    }

}