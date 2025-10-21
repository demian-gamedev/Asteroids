using CodeBase.Data;
using UnityEngine;

namespace CodeBase.Gameplay.Enviroment
{
    public interface IArenaMember
    {
        public TransformData TransformData { get; }
        public Transform transform { get; }
    }
}