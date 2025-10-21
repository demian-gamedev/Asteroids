using CodeBase.Data.StaticData;
using CodeBase.Interfaces.Infrastructure.Services;
using CodeBase.Interfaces.Infrastructure.Services.UI;
using Zenject;

namespace CodeBase.Infrastructure.Services.InputService
{
    public class InputService : IInputService, IInitializable
    {
        private readonly IMobileInputProvider _mobileInputProvider;
        private IInputStrategy _currentInputStrategy;

        private InputType _currentType;

        public InputService(IMobileInputProvider mobileInputProvider)
        {
            _mobileInputProvider = mobileInputProvider;
        }

        public void Initialize()
        {
            _currentInputStrategy = new MockInputStrategy();
        }

        public void SetInputType(InputType type)
        {
            if (_currentType == type) return;
            
            switch (type)
            {
                case InputType.Touchscreen:
                    _currentInputStrategy = new TouchscreenInputStrategy(_mobileInputProvider);
                    break;
                case InputType.Keyboard:
                    _currentInputStrategy = new KeyboardInputStrategy();
                    break;
            }
            _currentType = type;
        }

        public float GetMovement() => _currentInputStrategy.GetMovement();

        public float GetRotation() => _currentInputStrategy.GetRotation();

        public bool GetBaseAttack() => _currentInputStrategy.GetBaseAttack();

        public bool GetSpecialAttack() => _currentInputStrategy.GetSkill();
    }
}