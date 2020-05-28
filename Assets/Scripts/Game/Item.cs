using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 道具
/// </summary>
public class Item : MonoBehaviour {

    public GridPoint gridPoint;
    public int itemID;
    //private GameController.Instance GameController.Instance;
    private int prize;//销毁金币数额
    private int HP;
    private int currentHP;
    private Slider slider;

    private float timeVal;//显示或隐藏血条的计时器
    private bool showHp;
    
    private void OnEnable()
    {
        if (itemID!=0)
        {
#if Game
            InitItem();
#endif
        }
    }

    // Use this for initialization
    void Start () {

#if Tool
        GetComponent<BoxCollider2D>().enabled = false;
        transform.Find("Mask").GetComponent<BoxCollider>().enabled = false;
#endif
        //GameController.Instance = GameController.Instance.Instance;
        slider = transform.Find("ItemCanvas").Find("HpSlider").GetComponent<Slider>();
#if Game
        InitItem();
#endif
        slider.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (showHp)
        {
            if (timeVal<=0)
            {
                slider.gameObject.SetActive(false);
                showHp = false;
                timeVal = 3;
            }
            else
            {
                timeVal -= Time.deltaTime;
            }
        }
	}

#if Game

    private void TakeDamage(int attackValue)
    {
        slider.gameObject.SetActive(true);
        currentHP -= attackValue;
        if (currentHP<=0)
        {
            DestoryItem();
            return;
        }
        slider.value = (float)currentHP / HP;
        showHp = true;
        timeVal = 3;
    }

    private void DestoryItem()
    {
        if (GameController.Instance.targetTrans==transform)
        {
            GameController.Instance.HideSignal();
        }

        //金币奖励
        GameObject coinGo = GameController.Instance.GetGameObjectResource("CoinCanvas");
        coinGo.transform.Find("Emp_Coin").GetComponent<CoinMove>().prize = prize;
        coinGo.transform.SetParent(GameController.Instance.transform);
        coinGo.transform.position = transform.position;
        GameController.Instance.ChangeCoin(prize);

        //销毁特效
        GameObject effectGo = GameController.Instance.GetGameObjectResource("DestoryEffect");
        effectGo.transform.SetParent(GameController.Instance.transform);
        effectGo.transform.position = transform.position;

        GameController.Instance.PushGameObjectToFactory(GameController.Instance.mapMaker.bigLevelID.ToString()+"/Item/"+itemID,gameObject);

        gridPoint.gridState.hasItem = false;
        InitItem();

        GameController.Instance.PlayEffectMusic("NormalMordel/Item");
    }


    private void InitItem()
    {
        prize = 1000 - 100 * itemID;
        HP = 1500 - 100 * itemID;
        currentHP = HP;
        timeVal = 3;
        slider.gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (GameController.Instance.targetTrans == null)
        {
            GameController.Instance.targetTrans = transform;
            GameController.Instance.ShowSignal();
        }
        //转换新目标
        else if (GameController.Instance.targetTrans != transform)
        {
            GameController.Instance.HideSignal();
            GameController.Instance.targetTrans = transform;
            GameController.Instance.ShowSignal();
        }
        //两次点击的是同一个目标
        else if (GameController.Instance.targetTrans == transform)
        {
            GameController.Instance.HideSignal();
        }
    }
#endif
}
