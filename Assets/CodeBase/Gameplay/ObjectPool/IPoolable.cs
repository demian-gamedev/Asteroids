using System;

namespace CodeBase.Gameplay.ObjectPool
{
    public interface IPoolable
    {
        public Action<IPoolable> OnReturnToPool { get; set; }

    }
}