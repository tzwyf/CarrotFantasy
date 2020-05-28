using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilder<T> {

    //获取到游戏物体身上的脚本对象，从而去赋值
    T GetProductClass(GameObject gameObject);

    //使用工厂去获取具体的游戏对象
    GameObject GetProduct();

    //获取数据信息
    void GetData(T productClassGo);

    //获取特有资源与信息
    void GetOtherResource(T productClassGo);
}
