using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池
/// </summary>
public class ObjectPool : MonoBehaviour {

    //获取资源
    public GameObject monster;
    //怪物对象池
    private Stack<GameObject> monsterPool;
    //当前游戏世界里存在的或者说已经激活的怪物对象(只用于测试)
    private Stack<GameObject> activeMonsterList;

	// Use this for initialization
	void Start () {
        monsterPool = new Stack<GameObject>();
        activeMonsterList = new Stack<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            //如果我们需要游戏物体时我们要从池子管理里去取
            GameObject itemGo = GetMonster();
            //做一些后续工作.
            itemGo.transform.position = Vector3.one;
            activeMonsterList.Push(itemGo);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (activeMonsterList.Count>0)
            {
                //如果我们不使用或者销毁了当前怪物对象，
                //那么我们需要直接把怪物对象扔进池子
                PushMonster(activeMonsterList.Pop());
            }
        }
	}

    private GameObject GetMonster()
    {
        GameObject monsterGo = null;
        if (monsterPool.Count<=0)//池子里没有怪物对象
        {
            monsterGo = Instantiate(monster);
        }
        else//池子里有怪物对象
        {
            monsterGo = monsterPool.Pop();
            monsterGo.SetActive(true);
        }
        return monsterGo;
    }

    private void PushMonster(GameObject monsterGo)
    {
        monsterGo.transform.SetParent(transform);
        monsterGo.SetActive(false);
        monsterPool.Push(monsterGo);
    }
}
