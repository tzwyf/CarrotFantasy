using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 建塔按钮
/// </summary>
public class ButtonTower : MonoBehaviour {

    public int towerID;
    public int price;
    private Button button;
    private Sprite canClickSprite;
    private Sprite cantClickSprite;
    private Image image;
    //private GameController.Instance GameController.Instance;

    private void OnEnable()
    {
        if (price==0)
        {
            return;
        }
        UpdateIcon();
    }

    // Use this for initialization
    void Start () {
        //GameController.Instance = GameController.Instance.Instance;
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        canClickSprite = GameController.Instance.GetSprite("NormalMordel/Game/Tower/"+towerID.ToString()+"/CanClick1");
        cantClickSprite= GameController.Instance.GetSprite("NormalMordel/Game/Tower/" + towerID.ToString() + "/CanClick0");
        UpdateIcon();
        price = GameController.Instance.towerPriceDict[towerID];
        button.onClick.AddListener(BuildTower);
    }

    //建塔
    private void BuildTower()
    {
        GameController.Instance.PlayEffectMusic("NormalMordel/Tower/TowerBulid");
        //由建塔者去建造新塔
        GameController.Instance.towerBuilder.m_towerID = towerID;
        GameController.Instance.towerBuilder.m_towerLevel = 1;
        GameObject towerGo = GameController.Instance.towerBuilder.GetProduct();
        towerGo.transform.SetParent(GameController.Instance.selectGrid.transform);
        towerGo.transform.position = GameController.Instance.selectGrid.transform.position;
        //产生特效
        GameObject effectGo = GameController.Instance.GetGameObjectResource("BuildEffect");
        effectGo.transform.SetParent(GameController.Instance.transform);
        effectGo.transform.position = GameController.Instance.selectGrid.transform.position;
        //处理格子
#if Game
        GameController.Instance.selectGrid.AfterBuild();
        GameController.Instance.selectGrid.HideGrid();
#endif
        GameController.Instance.selectGrid.hasTower = true;
        GameController.Instance.ChangeCoin(-price);
        //不滞空会出现建完塔直接点击同一个格子不会显示按钮的情况
        GameController.Instance.selectGrid = null;
        //让操控画布先隐藏一次进行数值切换
        GameController.Instance.handleTowerCanvasGo.SetActive(false);
       
    }


    //更新图标
    private void UpdateIcon()
    {
        if (GameController.Instance.coin>price)
        {
            image.sprite = canClickSprite;
            button.interactable = true;
        }
        else
        {
            image.sprite = cantClickSprite;
            button.interactable = false;
        }
    }
}
