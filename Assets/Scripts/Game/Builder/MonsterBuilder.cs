using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBuilder : IBuilder<Monster>
{
    public int m_monsterID;
    private GameObject monsterGo;

    public void GetData(Monster productClassGo)
    {
        productClassGo.monsterID = m_monsterID;
        productClassGo.HP = m_monsterID * 100;
        productClassGo.currentHP = productClassGo.HP;
        productClassGo.initMoveSpeed = m_monsterID;
        productClassGo.moveSpeed = m_monsterID;
        productClassGo.prize = m_monsterID * 50;
    }

    public void GetOtherResource(Monster productClassGo)
    {
        productClassGo.GetMonsterProperty();
    }

    public GameObject GetProduct()
    {
        GameObject itemGo = GameController.Instance.GetGameObjectResource("MonsterPrefab");
        Monster monster = GetProductClass(itemGo);
        GetData(monster);
        GetOtherResource(monster);
        return itemGo;
    }

    public Monster GetProductClass(GameObject gameObject)
    {
        return gameObject.GetComponent<Monster>();
    }
}
