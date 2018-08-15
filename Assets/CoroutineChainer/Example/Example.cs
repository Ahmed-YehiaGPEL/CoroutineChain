using System.Collections;
using CoroutineChainer.Enums;
using UnityEngine;

namespace CoroutineChainer.Examples
{
    public class Example : MonoBehaviour
    {
        public Transform Cube1;
        public Transform Cube2;
        public float Duration = 0.4f;

        private ChainBase chain;

        private void Start()
        {
            chain = CoroutineChain.StartChain()
                .ChainWith(Cube1.Move(Vector3.up, Duration))
                .Wait(Duration)
                .ChainWith(Cube1.Move(Vector3.zero, Duration))
                .Wait(Duration)
                .ChainWith(Cube2.Move(Vector2.one, Duration))
                .Wait(Duration)
                .ChainWith(Cube2.Move(Vector3.right, Duration))
                .Wait(Duration)
                .Parallel(Cube1.Move(Vector3.up, Duration), Cube2.Move(Vector2.one, Duration))
                .Log("Parallel Complete!")
                .Wait(Duration)
                .Sequential(Cube1.Move(Vector3.zero, Duration), Cube2.Move(Vector3.right, Duration))
                .Log("Sequential Complete", ELogType.Normal);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.A))
            {
                chain.RunCoroutine();
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                CoroutineChain.StopAll();
            }
        }
    }

    public static class TransformExtensions
    {
        public static IEnumerator Move(this Transform trans, Vector3 worldPosition, float sec)
        {
            Vector3 start = trans.position;
            float t = 0f;
            while (t < 1f)
            {
                t = Mathf.Min(1f, t + Time.deltaTime / sec);
                trans.position = Vector3.Lerp(start, worldPosition, t);
                yield return null;
            }
        }
    }
}