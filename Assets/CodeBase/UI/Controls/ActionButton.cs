using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeBase.UI.Controls
{
    public class ActionButton : MonoBehaviour, IPointerDownHandler
    {
        public bool IsPressed { get; private set; }

        private void Update()
        {
            if (IsPressed)
            {
                IsPressed = false;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsPressed = true;
        }
        private void OnDisable()
        {
            IsPressed = false;
        }
    }
}