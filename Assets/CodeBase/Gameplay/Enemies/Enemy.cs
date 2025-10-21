using System;
using CodeBase.Data;
using CodeBase.Gameplay.Enviroment;
using CodeBase.Gameplay.ObjectPool;
using UnityEngine;

namespace CodeBase.Gameplay.Enemies
{
    public abstract class Enemy : MonoBehaviour, IArenaMember, IPoolable
    {
        public TransformData TransformData { get; protected set; }
        public Action<IPoolable> OnReturnToPool { get; set; }
        
        protected void ReturnToPool()
        {
            OnReturnToPool?.Invoke(this);
        }
    }
}