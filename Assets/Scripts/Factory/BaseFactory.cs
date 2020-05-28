using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏物体类型的工厂基类
/// </summary>
public class BaseFactory : IBaseFacotry
{
    //当前拥有的gameObject类型的资源(UI,UIPanel,Game) 切记：它存放的是游戏物体资源
    protected Dictionary<string, GameObject> facotryDict = new Dictionary<string, GameObject>();
    //对象池字典
    protected Dictionary<string, Stack<GameObject>> objectPoolDict = new Dictionary<string, Stack<GameObject>>();
    //对象池(就是我们具体存贮的游戏物体类型的对象)注：对应的是一个具体的游戏物体对象

    //加载路径
    protected string loadPath;

    public BaseFactory()
    {
        loadPath = "Prefabs/";
    }

    //放入池子
    public void PushItem(string itemName, GameObject item)
    {
        item.SetActive(false);
        item.transform.SetParent(GameManager.Instance.transform);
        if (objectPoolDict.ContainsKey(itemName))
        {
            objectPoolDict[itemName].Push(item);
        }
        else
        {
            Debug.Log("当前字典没有"+itemName+"的栈");
        }
    }

    //取实例
    public GameObject GetItem(string itemName)
    {
        GameObject itemGo = null;
        if (objectPoolDict.ContainsKey(itemName))//包含此对象池
        {
            if (objectPoolDict[itemName].Count==0)
            {
                GameObject go = GetResource(itemName);
                itemGo = GameManager.Instance.CreateItem(go);
            }
            else
            {
                itemGo = objectPoolDict[itemName].Pop();
                itemGo.SetActive(true);
            }
        }
        else//不包含此对象池
        {
            objectPoolDict.Add(itemName,new Stack<GameObject>());
            GameObject go = GetResource(itemName);
            itemGo = GameManager.Instance.CreateItem(go);
        }

        if (itemGo==null)
        {
            Debug.Log(itemName+"的实例获取失败");
        }

        return itemGo;
    }

    //取资源
    private GameObject GetResource(string itemName)
    {
        GameObject itemGo = null;
        string itemLoadPath = loadPath + itemName;
        if (facotryDict.ContainsKey(itemName))
        {
            itemGo = facotryDict[itemName];
        }
        else
        {
            itemGo = Resources.Load<GameObject>(itemLoadPath);
            facotryDict.Add(itemName,itemGo);
        }
        if (itemGo==null)
        {
            Debug.Log(itemName+"的资源获取失败");
            Debug.Log("失败路径："+itemLoadPath);
        }
        return itemGo;
    }
}
