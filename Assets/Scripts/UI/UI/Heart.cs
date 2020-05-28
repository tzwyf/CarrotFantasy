using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 心脏效果
/// </summary>
public class Heart : MonoBehaviour {

    public float animationTime;
    public string resourcePath;

    private void OnEnable()
    {
        Invoke("DestoryEffect", animationTime);
    }

    private void DestoryEffect()
    {
        GameManager.Instance.factoryManager.factoryDict[FactoryType.UIFactory].PushItem(resourcePath, gameObject);
    }
}
