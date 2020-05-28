using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 卖塔按钮
/// </summary>
public class ButtonSellTower : MonoBehaviour {

    private int price;
    private Button button;
    private Text text;
    //private GameController.Instance GameController.Instance;

	// Use this for initialization
	void Start () {
        //GameController.Instance = GameController.Instance.Instance;
        button = GetComponent<Button>();
        button.onClick.AddListener(SellTower);
        text = transform.Find("Text").GetComponent<Text>();
    }

    private void OnEnable()
    {
        if (text==null)
        {
            return;
        }
        price = GameController.Instance.selectGrid.towerPersonalProperty.sellPrice;
        text.text = price.ToString();
    }

    private void SellTower()
    {
        GameController.Instance.selectGrid.towerPersonalProperty.SellTower();
        GameController.Instance.selectGrid.InitGrid();
        GameController.Instance.selectGrid.handleTowerGanvasGo.SetActive(false);
#if Game
        GameController.Instance.selectGrid.HideGrid();
#endif
        GameController.Instance.selectGrid = null;
    }
}
