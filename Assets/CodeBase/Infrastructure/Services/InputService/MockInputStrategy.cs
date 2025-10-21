namespace CodeBase.Infrastructure.Services.InputService
{
    public class MockInputStrategy : IInputStrategy
    {
        public float GetMovement() => 0f;

        public float GetRotation() => 0f;

        public bool GetBaseAttack() => false;

        public bool GetSkill() => false;
    }
}