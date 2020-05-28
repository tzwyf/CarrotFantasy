using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 特效脚本
/// </summary>
public class Effect : MonoBehaviour {

    public float animationTime;
    public string resourcePath;

    private void OnEnable()
    {
        Invoke("DestoryEffect",animationTime);
    }

    private void DestoryEffect()
    {
        GameController.Instance.PushGameObjectToFactory(resourcePath,gameObject);
    }
}
