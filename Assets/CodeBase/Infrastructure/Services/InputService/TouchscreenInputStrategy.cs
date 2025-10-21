using CodeBase.Interfaces.Infrastructure.Services.UI;

namespace CodeBase.Infrastructure.Services.InputService
{
    public class TouchscreenInputStrategy : IInputStrategy
    {
        private readonly IMobileInputProvider _mobileInputProvider;

        public TouchscreenInputStrategy(IMobileInputProvider mobileInputProvider)
        {
            _mobileInputProvider = mobileInputProvider;
        }
        public float GetMovement() => _mobileInputProvider.MobileInput.Movement;

        public float GetRotation() => -_mobileInputProvider.MobileInput.Rotation;

        public bool GetBaseAttack() => _mobileInputProvider.MobileInput.Attack;

        public bool GetSkill() => _mobileInputProvider.MobileInput.Skill;
    }
}