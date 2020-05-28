using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏物体工厂的接口
/// </summary>
public interface IBaseFacotry  {

    GameObject GetItem(string itemName);

    void PushItem(string itemName,GameObject item);
}
