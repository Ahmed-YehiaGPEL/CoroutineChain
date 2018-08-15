using UnityEngine;
using Object = UnityEngine.Object;

namespace CoroutineChainer
{
    public static class CoroutineChain
    {
        private static Dispatcher dispatcherInstance;

        private static Dispatcher Instance
        {
            get
            {
                if (dispatcherInstance == null)
                {
                    dispatcherInstance = new GameObject("CoroutineChain").AddComponent<Dispatcher>();
                    Object.DontDestroyOnLoad(dispatcherInstance);
                }
                return dispatcherInstance;
            }
        }

        public static void StopAll()
        {
            dispatcherInstance.StopAllCoroutines();
        }

        public static ChainBase StartChain()
        {
            return ChainBase.BasePool.Spawn(Instance);
        }
    }
}