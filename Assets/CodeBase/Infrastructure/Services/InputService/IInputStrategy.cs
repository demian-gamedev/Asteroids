namespace CodeBase.Infrastructure.Services.InputService
{
    public interface IInputStrategy
    {
        public float GetMovement();

        public float GetRotation();

        public bool GetBaseAttack();

        public bool GetSkill();
    }
}