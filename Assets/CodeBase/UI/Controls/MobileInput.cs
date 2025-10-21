using System;
using CodeBase.Interfaces.Infrastructure.Services.UI;
using CodeBase.UI.Controls.CodeBase.UI.Controls;
using UnityEngine;

namespace CodeBase.UI.Controls
{
    public class MobileInput : MonoBehaviour, IMobileInput
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private ActionButton _attack;
        [SerializeField] private ActionButton _skill;

        public Vector2 MoveDirection => _joystick.Direction;
        public float Movement => MoveDirection.y;
        public float Rotation => MoveDirection.x;
        public bool Attack => _attack.IsPressed;
        public bool Skill => _skill.IsPressed;
        public event Action Destroyed;
        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        private void OnDestroy()
        {
            Destroyed?.Invoke();
        }
    }
}