using CoroutineChainer;
using UnityEngine;

public static class MonobehaviourExtensions
{    
    public static ChainBase StartChain(this MonoBehaviour mono)
    {
        return ChainBase.BasePool.Spawn(mono);
    }
}