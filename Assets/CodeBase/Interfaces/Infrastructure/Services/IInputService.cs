using CodeBase.Data.StaticData;

namespace CodeBase.Interfaces.Infrastructure.Services
{
    public interface IInputService
    {
        public void SetInputType(InputType type);
        float GetMovement();
        float GetRotation();
        bool GetBaseAttack();
        bool GetSpecialAttack();
    }
}