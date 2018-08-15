using System;
using System.Collections;
using CoroutineChainer.Enums;
using UnityEngine;

namespace CoroutineChainer
{
    public class Chain
    {
        private EType type;
        private MonoBehaviour player;
        private IEnumerator routine;
        private IEnumerator[] parallelRoutine;
        private Action action;

        public Coroutine Play()
        {
            switch (type)
            {
                default:
                case EType.NonCoroutine:
                    action();
                    return null;
                case EType.Parallel:
                    return player.StartCoroutine(Parallel(parallelRoutine));
                case EType.Single:
                    return player.StartCoroutine(routine);
            }
        }

        public Chain SetupRoutine(IEnumerator routine, MonoBehaviour player)
        {
            type = EType.Single;
            this.player = player;
            this.routine = routine;
            return this;
        }

        public Chain SetupParallel(IEnumerator[] routines, MonoBehaviour player)
        {
            type = EType.Parallel;
            this.player = player;
            this.parallelRoutine = routines;
            return this;
        }

        public Chain SetupNon(System.Action action, MonoBehaviour player)
        {
            type = EType.NonCoroutine;
            this.player = player;
            this.action = action;
            return this;
        }

        public void Clear()
        {
            player = null;
            routine = null;
            action = null;
            parallelRoutine = null;
        }

        private IEnumerator Parallel(IEnumerator[] routines)
        {
            int all = 0;
            all += routines.Length;
            int c = 0;
            foreach (IEnumerator r in routines)
                player.StartChain()
                    .ChainWith(r)
                    .Call(() => c++)
                    .RunCoroutine();

            while (c < all)
                yield return null;
        }

        
    }
}