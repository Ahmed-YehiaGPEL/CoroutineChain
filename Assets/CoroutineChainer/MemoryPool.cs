using System;
using System.Collections.Generic;

namespace CoroutineChainer
{
    public class MemoryPool<T> where T : new()
    {
        private readonly Stack<T> pool = new Stack<T>();
        private readonly Action<T> onSpawnAction;
        private readonly Action<T> onDespawnAction;

        public MemoryPool(Action<T> onSpawnAction = null, Action<T> onDespawnAction = null)
        {
            this.onDespawnAction = onDespawnAction;
            this.onSpawnAction = onSpawnAction;
        }

        public T Spawn()
        {
            T item = pool.Count == 0 ? new T() : pool.Pop();
            onSpawnAction?.Invoke(item);
            return item;
        }

        public void Despawn(T item)
        {
            onDespawnAction?.Invoke(item);
            pool.Push(item);
        }
    }

    public class MemoryPool<T, TParam> where T : new()
    {
        private readonly Stack<T> pool = new Stack<T>();

        private readonly Action<T, TParam> onSpawnAction;
        private readonly Action<T> onDespawnAction;

        public MemoryPool(Action<T, TParam> onSpawnAction = null, Action<T> onDespawnAction = null)
        {
            this.onDespawnAction = onDespawnAction;
            this.onSpawnAction = onSpawnAction;
        }

        public T Spawn(TParam init)
        {
            T item = pool.Count == 0 ? new T() : pool.Pop();
            onSpawnAction?.Invoke(item, init);
            return item;
        }

        public void Despawn(T item)
        {
            onDespawnAction?.Invoke(item);
            pool.Push(item);
        }
    }
}