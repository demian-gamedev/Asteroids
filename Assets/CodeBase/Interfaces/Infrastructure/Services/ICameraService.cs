using UnityEngine;

namespace CodeBase.Interfaces.Infrastructure.Services
{
    public interface ICameraService
    {
        public Camera Camera { get; }
        void Follow(Transform target);
    }
}