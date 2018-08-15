using UnityEngine;

namespace CoroutineChainer.Examples
{
    public class WaitAndCall : MonoBehaviour
    {
        // Use this for initialization
        private void Start()
        {
            this.StartChain()
                .Call(() => Debug.Log("1"))
                .Wait(1)
                .Log("2")
                .Wait(1)
                .Log("end")
                .RunCoroutine();
        }
    }
}
