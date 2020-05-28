using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 塔的共同特性
/// </summary>
public class Tower : MonoBehaviour {

    public int towerID;
    private CircleCollider2D circleCollider2D;//攻击检测范围
    private TowerPersonalProperty towerPersonalProperty;//塔的特异性功能脚本
    private SpriteRenderer attackRangeSr;//攻击范围渲染
    public bool isTarget;//是集火目标
    public bool hasTarget;//有目标

    // Use this for initialization
    void Start() {
        Init();
    }

    private void OnEnable()
    {
        Init();
    }

    // Update is called once per frame
    void Update() {
        if (GameController.Instance.isPause||GameController.Instance.gameOver)
        {
            return;
        }

        if (isTarget)
        {
            if (towerPersonalProperty.targetTrans!=GameController.Instance.targetTrans)
            {
                towerPersonalProperty.targetTrans = null;
                hasTarget = false;
                isTarget = false;
            }
        }
        if (hasTarget)
        {
            if (!towerPersonalProperty.targetTrans.gameObject.activeSelf)
            {
                towerPersonalProperty.targetTrans = null;
                hasTarget = false;
                isTarget = false;
            }
        }
	}

    //初始化方法
    private void Init()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        towerPersonalProperty = GetComponent<TowerPersonalProperty>();
        towerPersonalProperty.tower = this;
        attackRangeSr = transform.Find("attackRange").GetComponent<SpriteRenderer>();
        attackRangeSr.gameObject.SetActive(false);
        attackRangeSr.transform.localScale = new Vector3(towerPersonalProperty.towerLevel, towerPersonalProperty.towerLevel, 1);
        circleCollider2D.radius = 1.1f*towerPersonalProperty.towerLevel;
        isTarget = false;
        hasTarget = false;
    }

    public void GetTowerProperty()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag!="Monster"&&collision.tag!="Item"&&isTarget)
        {
            return;
        }
        //集火目标
        Transform targetTrans = GameController.Instance.targetTrans;
        if (targetTrans!=null)//有集火目标
        {
            if (!isTarget)//还没找到
            {
                //是物品且是集火目标
                if (collision.tag=="Item"&&collision.transform==targetTrans)
                {
                    towerPersonalProperty.targetTrans = collision.transform;
                    isTarget = true;
                    hasTarget = true;
                }
                //是怪物
                else if (collision.tag=="Monster")
                {
                    //且是集火目标
                    if (collision.transform==targetTrans)
                    {
                        towerPersonalProperty.targetTrans = collision.transform;
                        isTarget = true;
                        hasTarget = true;
                    }
                    //不是集火目标
                    else if (collision.transform!=targetTrans&&!hasTarget)
                    {
                        towerPersonalProperty.targetTrans = collision.transform;
                        hasTarget = true;
                    }
                }
            }
        }
        else
        {
            if (!hasTarget&&collision.tag=="Monster")
            {
                towerPersonalProperty.targetTrans = collision.transform;
                hasTarget = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.tag != "Monster" && collision.tag != "Item" && isTarget)
        {
            return;
        }
        //集火目标
        Transform targetTrans = GameController.Instance.targetTrans;
        if (targetTrans != null)//有集火目标
        {
            if (!isTarget)//还没找到
            {
                //是物品且是集火目标
                if (collision.tag == "Item" && collision.transform == targetTrans)
                {
                    towerPersonalProperty.targetTrans = collision.transform;
                    isTarget = true;
                    hasTarget = true;
                }
                //是怪物
                else if (collision.tag == "Monster")
                {
                    //且是集火目标
                    if (collision.transform == targetTrans)
                    {
                        towerPersonalProperty.targetTrans = collision.transform;
                        isTarget = true;
                        hasTarget = true;
                    }
                    //不是集火目标
                    else if (collision.transform != targetTrans && !hasTarget)
                    {
                        towerPersonalProperty.targetTrans = collision.transform;
                        hasTarget = true;
                    }
                }
            }
        }
        else
        {
            if (!hasTarget && collision.tag == "Monster")
            {
                towerPersonalProperty.targetTrans = collision.transform;
                hasTarget = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (towerPersonalProperty.targetTrans == collision.transform)
        {
            towerPersonalProperty.targetTrans = null;
            hasTarget = false;
            isTarget = false;
        }
    }

    public void DestoryTower()
    {
        towerPersonalProperty.Init();
        Init();
        GameController.Instance.PushGameObjectToFactory("Tower/ID"+towerID.ToString()+"/TowerSet/"+towerPersonalProperty.towerLevel.ToString(),gameObject);
    }
}
