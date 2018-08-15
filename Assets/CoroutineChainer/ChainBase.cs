using System;
using System.Collections;
using System.Collections.Generic;
using CoroutineChainer.Enums;
using UnityEngine;

namespace CoroutineChainer
{
    public class ChainBase : CustomYieldInstruction
    {
        public static readonly MemoryPool<ChainBase, MonoBehaviour> BasePool = new MemoryPool<ChainBase, MonoBehaviour>((c, m) => c.Setup(m), c => c.Clear());

        private static readonly MemoryPool<Chain> ChainPool = new MemoryPool<Chain>(null, c => c.Clear());

        private MonoBehaviour coroutineRunner;

        private readonly Queue<Chain> chainQueue = new Queue<Chain>();

        private bool isRunning = true;

        public override bool keepWaiting
        {
            get
            {
                return isRunning;
            }
        }
        public ChainBase ChainWith(IEnumerator routine)
        {
            chainQueue.Enqueue(ChainPool.Spawn().SetupRoutine(routine, coroutineRunner));
            return this;
        }

        public ChainBase Wait(float waitSec)
        {
            chainQueue.Enqueue(ChainPool.Spawn().SetupRoutine(WaitRoutine(waitSec), coroutineRunner));
            return this;
        }

        public ChainBase Parallel(params IEnumerator[] routines)
        {
            chainQueue.Enqueue(ChainPool.Spawn().SetupParallel(routines, coroutineRunner));
            return this;
        }

        public ChainBase Sequential(params IEnumerator[] routines)
        {
            foreach (IEnumerator routine in routines)
                chainQueue.Enqueue(ChainPool.Spawn().SetupRoutine(routine, coroutineRunner));
            return this;
        }

        public ChainBase Log(string log, ELogType type = ELogType.Normal)
        {
            Action action;
            switch (type)
            {
                default:
                case ELogType.Normal:
                    action = () => Debug.Log(log); break;
                case ELogType.Warrning:
                    action = () => Debug.LogWarning(log); break;
                case ELogType.Error:
                    action = () => Debug.LogError(log); break;
            }
            chainQueue.Enqueue(ChainPool.Spawn().SetupNon(action, coroutineRunner));
            return this;
        }

        public ChainBase Call(Action action)
        {
            chainQueue.Enqueue(ChainPool.Spawn().SetupNon(action, coroutineRunner));
            return this;
        }

        public Coroutine RunCoroutine()
        {
            return coroutineRunner?.StartCoroutine(Routine());
        }

        private ChainBase Setup(MonoBehaviour player)
        {
            isRunning = true;
            coroutineRunner = player;
            return this;
        }

        private void Clear()
        {
            coroutineRunner = null;
            chainQueue.Clear();
        }

        private IEnumerator Routine()
        {
            yield return null;

            while (chainQueue.Count > 0)
            {
                Chain chain = chainQueue.Dequeue();
                Coroutine cr = chain.Play();
                if (cr != null)
                    yield return cr;
                ChainPool.Despawn(chain);
            }

            isRunning = false;
            BasePool.Despawn(this);
        }

        private IEnumerator WaitRoutine(float wait)
        {
            yield return new WaitForSeconds(wait);
        }

       
    }
}