using UnityEngine;

namespace CodeBase.Interfaces.Infrastructure.Services
{
    public interface IRandomizerService
    {
        Vector2 GetRandomPositionOnBoundsEdge(Vector2 boundsSize, Vector2 center, float additionalOffset);
        float Range(float from, float to);
        float RandomRotation();
        Vector2 RandomDirection();
    }
}