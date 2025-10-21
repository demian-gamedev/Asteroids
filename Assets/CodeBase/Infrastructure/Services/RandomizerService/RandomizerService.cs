using CodeBase.Interfaces.Infrastructure.Services;
using UnityEngine;
using Random = System.Random;

namespace CodeBase.Infrastructure.Services.RandomizerService
{
    public class RandomizerService : IRandomizerService
    {
        private readonly Random _random;

        public RandomizerService()
        {
            _random = new System.Random();
        }

        public Vector2 GetRandomPositionOnBoundsEdge(Vector2 boundsSize, Vector2 center, float additionalOffset)
        {
            float x = 0f;
            float y = 0f;

            // Randomly choose whether to place on horizontal or vertical edge
            if (_random.Next(2) == 0) // Horizontal edge
            {
                x = _random.Next(-Mathf.RoundToInt(boundsSize.x / 2), Mathf.RoundToInt(boundsSize.x / 2));
                y = _random.Next(2) == 0 ? -boundsSize.y / 2 : boundsSize.y / 2; // Top or bottom edge
            }
            else // Vertical edge
            {
                y = _random.Next(-Mathf.RoundToInt(boundsSize.y / 2), Mathf.RoundToInt(boundsSize.y / 2));
                x = _random.Next(2) == 0 ? -boundsSize.x / 2 : boundsSize.x / 2; // Left or right edge
            }

            Vector2 res = new Vector2(x, y);
            
            res += res.normalized * additionalOffset;
            
            return res + center;
        }

        public float Range(float from, float to)
        {
            return (float)(_random.NextDouble() * (to - from) + from);
        }

        public float RandomRotation()
        {
            return Range(0, 360f);
        }

        public Vector2 RandomDirection()
        {
            float angle = Range(0f, 360f);
            float radian = angle * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)).normalized;
        }
    }
}