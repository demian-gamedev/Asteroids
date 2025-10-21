using UnityEngine;

namespace CodeBase.Gameplay
{
    public interface IPushable
    {
        public void Push(Vector2 forceVector);
    }
}