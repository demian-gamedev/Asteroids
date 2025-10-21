using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeBase.UI.Controls
{
    namespace CodeBase.UI.Controls
    {
        public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
        {
            [SerializeField] private RectTransform _background;
            [SerializeField] private RectTransform _handle;
            [SerializeField] private float _maxOffset = 100f;

            private Vector2 _inputVector;
            private bool _isActive;

            public Vector2 Direction => _inputVector;

            private void Awake()
            {
                _handle.anchoredPosition = Vector2.zero;
            }

            public void OnPointerDown(PointerEventData eventData)
            {
                _isActive = true;
                OnDrag(eventData);
            }

            public void OnDrag(PointerEventData eventData)
            {
                Vector2 position;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _background,
                    eventData.position,
                    eventData.pressEventCamera,
                    out position);

                position = Vector2.ClampMagnitude(position, _maxOffset);
                _handle.anchoredPosition = position;

                _inputVector = position / _maxOffset;
            }

            public void OnPointerUp(PointerEventData eventData)
            {
                _isActive = false;
                _inputVector = Vector2.zero;
                _handle.anchoredPosition = Vector2.zero;
            }
        }
    }
}