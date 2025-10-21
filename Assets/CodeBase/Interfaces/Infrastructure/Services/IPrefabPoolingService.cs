using UnityEngine;

namespace CodeBase.Interfaces.Infrastructure.Services
{
    public interface IPrefabPoolingService
    {
        GameObject Spawn(GameObject prefab, Transform parent = null);
        void Despawn(GameObject gameObject);
        TComponent Spawn<TComponent>(TComponent prefab, Transform parent = null) where TComponent : MonoBehaviour;
        void Despawn<TComponent>(TComponent gameObject) where TComponent : MonoBehaviour;
    }
}