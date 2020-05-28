using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 升级按钮
/// </summary>
public class ButtonUpLevel : MonoBehaviour {

    private int price;
    private Button button;
    private Text text;
    private Image image;
    private Sprite canUpLevelSprite;
    private Sprite cantUpLevelSprite;
    private Sprite reachHighestLevel;
    //private GameController.Instance GameController.Instance;

    private void OnEnable()
    {
        if (text==null)
        {
            return;
        }
        UpdateUIView();
    }

    // Use this for initialization
    void Start () {
#if Game
        //GameController.Instance = GameController.Instance.Instance;
        button = GetComponent<Button>();
        button.onClick.AddListener(UpLevel);
        canUpLevelSprite = GameController.Instance.GetSprite("NormalMordel/Game/Tower/Btn_CanUpLevel");
        cantUpLevelSprite = GameController.Instance.GetSprite("NormalMordel/Game/Tower/Btn_CantUpLevel");
        reachHighestLevel = GameController.Instance.GetSprite("NormalMordel/Game/Tower/Btn_ReachHighestLevel");
        text = transform.Find("Text").GetComponent<Text>();
        image = GetComponent<Image>();
#endif
    }

    private void UpdateUIView()
    {
        if (GameController.Instance.selectGrid.towerLevel>=3)
        {
            image.sprite = reachHighestLevel;
            button.interactable = false;
            text.enabled = false;
        }
        else
        {
            text.enabled = true;
            price = GameController.Instance.selectGrid.towerPersonalProperty.upLevelPrice;
            text.text = price.ToString();
            if (GameController.Instance.coin>=price)
            {
                image.sprite = canUpLevelSprite;
                button.interactable = true;
            }
            else
            {
                image.sprite = cantUpLevelSprite;
                button.interactable = false;
            }
        }
    }

    private void UpLevel()
    {
        //赋值建造要产生的塔的属性
        GameController.Instance.towerBuilder.m_towerID = GameController.Instance.selectGrid.tower.towerID;
        GameController.Instance.towerBuilder.m_towerLevel = GameController.Instance.selectGrid.towerLevel + 1;
        //销毁之前的等级的塔
        GameController.Instance.selectGrid.towerPersonalProperty.UpLevelTower();
        //产生新塔
        GameObject towerGo = GameController.Instance.towerBuilder.GetProduct();
        towerGo.transform.SetParent(GameController.Instance.selectGrid.transform);
        towerGo.transform.position = GameController.Instance.selectGrid.transform.position;
#if Game
        GameController.Instance.selectGrid.AfterBuild();
        GameController.Instance.selectGrid.HideGrid();
#endif
        GameController.Instance.selectGrid = null;
    }
}
