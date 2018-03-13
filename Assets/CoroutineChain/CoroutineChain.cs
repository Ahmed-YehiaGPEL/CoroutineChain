﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using geniikw.CChain;

public static class MonobehaviourExtend
{    
    public static ChainBase StartChain(this MonoBehaviour mono)
    {
        return ChainBase.BasePool.Spawn(mono);
    }
}

/// <summary>
/// 호환성을 위해 이름유지.
/// </summary>
public static class CoroutineChain
{
    class Dispather : MonoBehaviour { }
    static Dispather m_instance;
    static Dispather Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new GameObject("CoroutineChain").AddComponent<Dispather>();
                Object.DontDestroyOnLoad(m_instance);
            }
            return m_instance;
        }
    }

    public static void StopAll()
    {
        m_instance.StopAllCoroutines();
    }
    
    public static ChainBase Start
    {
        get
        {
            return ChainBase.BasePool.Spawn(Instance);
        }
    }

}
