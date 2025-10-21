using System;
using System.Collections.Generic;

namespace CodeBase.Gameplay.ObjectPool
{
    public class ObjectPool<T> where T : IPoolable
    {
        private readonly Stack<T> _pool;
        private readonly Func<T> _createFunc;
        private readonly Action<T> _onGet;
        private readonly Action<T> _onRelease;
        private readonly Action<T> _onDestroy;
        private readonly int _maxSize;

        public int CountAll { get; private set; }
        public int CountActive => CountAll - CountInactive;
        public int CountInactive => _pool.Count;

        public ObjectPool(
            Func<T> createFunc,
            Action<T> onGet = null,
            Action<T> onRelease = null,
            Action<T> onDestroy = null,
            int defaultCapacity = 10,
            int maxSize = 10000)
        {
            if (createFunc == null)
                throw new ArgumentNullException(nameof(createFunc));

            _pool = new Stack<T>(defaultCapacity);
            _createFunc = createFunc;
            _onGet = onGet;
            _onRelease = onRelease;
            _onDestroy = onDestroy;
            _maxSize = maxSize;
        }

        public void PreWarm(int count)
        {
            if (count <= 0)
                throw new ArgumentException("Count must be greater than zero", nameof(count));
            
            int objectsToCreate = Math.Min(count, _maxSize - _pool.Count);
            
            for (int i = 0; i < objectsToCreate; i++)
            {
                T obj = _createFunc();
                obj.OnReturnToPool += (poolable) => Release((T)poolable);
                Release(obj); // Ensure the object is unactive
                CountAll++;
            }
        }

        public T Get()
        {
            T obj;
            if (_pool.Count == 0)
            {
                if (CountAll >= _maxSize)
                {
                    throw new InvalidOperationException($"Pool reached maximum size of {_maxSize}");
                }
                
                obj = _createFunc();
                CountAll++;
            }
            else
            {
                obj = _pool.Pop();
            }
            
            _onGet?.Invoke(obj);
            return obj;
        }

        public void Release(T element)
        {
            if (_pool.Contains(element)) return;

            _onRelease?.Invoke(element);

            if (_pool.Count < _maxSize)
            {
                _pool.Push(element);
            }
            else
            {
                _onDestroy?.Invoke(element);
                CountAll--;
            }
        }

        public void Clear()
        {
            if (_onDestroy != null)
            {
                foreach (T item in _pool)
                {
                    _onDestroy(item);
                }
            }
            
            _pool.Clear();
            CountAll = 0;
        }
    }
}