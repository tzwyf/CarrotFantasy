using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 塔的建塔者
/// </summary>
public class TowerBuilder : IBuilder<Tower>
{
    public int m_towerID;
    private GameObject towerGo;
    public int m_towerLevel;

    public void GetData(Tower productClassGo)
    {
        productClassGo.towerID = m_towerID;
    }

    public void GetOtherResource(Tower productClassGo)
    {
        productClassGo.GetTowerProperty();
    }

    public GameObject GetProduct()
    {
        GameObject gameObject = GameController.Instance.GetGameObjectResource("Tower/ID"+m_towerID.ToString()+"/TowerSet/"+m_towerLevel.ToString());
        Tower tower = GetProductClass(gameObject);
        GetData(tower);
        GetOtherResource(tower);
        return gameObject;
    }

    public Tower GetProductClass(GameObject gameObject)
    {
        return gameObject.GetComponent<Tower>();
    }
}
