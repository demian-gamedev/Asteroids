using CodeBase.Interfaces.Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Services.Camera
{
    public class CameraService : ICameraService, ILateTickable
    {
        private readonly UnityEngine.Camera _camera;
        private Transform _target;

        public UnityEngine.Camera Camera => _camera;
        public CameraService(UnityEngine.Camera camera)
        {
            _camera = camera;
        }

        public void Follow(Transform target)
        {
            _target = target;
        }

        public void LateTick()
        {
            if (_target != null)
            {
                _camera.transform.position = new Vector3(
                    _target.position.x,
                    _target.position.y,
                    _camera.transform.position.z
                );
            }
        }
    }
}