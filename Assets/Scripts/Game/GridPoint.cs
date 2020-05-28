using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

/// <summary>
/// 格子脚本：存贮塔，塔子信息，以及玩家与塔格子交互的方法
/// </summary>
public class GridPoint : MonoBehaviour {

    //属性
    private SpriteRenderer spriteRenderer;
    public GridState gridState;
    public GridIndex gridIndex;
    public bool hasTower;

    //资源
    private Sprite gridSprite;//格子图片资源
    private Sprite startSprite;//开始时格子的图片显示
    private Sprite cantBuildSprite;//禁止建塔

#if Tool
    private Sprite monsterPathSprite;//怪物路点图片资源
    public GameObject[] itemPrefabs;//道具数组
    public GameObject currentItem;//当前格子持有道具
#endif
    //格子状态
    [System.Serializable]
    public struct GridState
    {
        public bool canBuild;
        public bool isMonsterPoint;
        public bool hasItem;
        public int itemID;
    }

    //格子索引
    [System.Serializable]
    public struct GridIndex
    {
        public int xIndex;
        public int yIndex;
    }

    //引用
    //private GameController.Instance GameController.Instance;
    private GameObject towerListGo;//当前关卡建塔列表
    public GameObject handleTowerGanvasGo;//有塔之后的操作按钮画布
    private Transform upLevelButtonTrans;//两个按钮的trans引用
    private Transform sellTowerButtonTrans;
    private Vector3 upLevelButtonInitPos;//两个按钮的初始位置
    private Vector3 sellTowerButtonInitPos;

    //有塔之后的属性
    public GameObject towerGo;
    public Tower tower;
    public TowerPersonalProperty towerPersonalProperty;
    public int towerLevel;
    private GameObject levelUpSignalGo;//是否可升级信号

    private void Awake()
    {
#if Tool
        gridSprite= Resources.Load<Sprite>("Pictures/NormalMordel/Game/Grid");
        monsterPathSprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/1/Monster/6-1");
        itemPrefabs = new GameObject[10];
        string prefabsPath = "Prefabs/Game/" + MapMaker.Instance.bigLevelID.ToString()+"/Item/";
        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            itemPrefabs[i] = Resources.Load<GameObject>(prefabsPath + i);
            if (!itemPrefabs[i])
            {
                Debug.Log("加载失败，失败路径："+prefabsPath+i); 
            }
        }
#endif
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitGrid();
#if Game
        //GameController.Instance = GameController.Instance.Instance;
        gridSprite = GameController.Instance.GetSprite("NormalMordel/Game/Grid");
        startSprite = GameController.Instance.GetSprite("NormalMordel/Game/StartSprite");
        cantBuildSprite = GameController.Instance.GetSprite("NormalMordel/Game/cantBuild");
        spriteRenderer.sprite = startSprite;
        Tween t = DOTween.To(() => spriteRenderer.color, toColor => spriteRenderer.color=toColor, new Color(1, 1, 1, 0.2f), 3);
        t.OnComplete(ChangeSpriteToGrid);
        towerListGo = GameController.Instance.towerListGo;
        handleTowerGanvasGo = GameController.Instance.handleTowerCanvasGo;
        upLevelButtonTrans = handleTowerGanvasGo.transform.Find("Btn_UpLevel");
        sellTowerButtonTrans = handleTowerGanvasGo.transform.Find("Btn_SellTower");
        upLevelButtonInitPos = upLevelButtonTrans.localPosition;
        sellTowerButtonInitPos = sellTowerButtonTrans.localPosition;
        levelUpSignalGo = transform.Find("LevelUpSignal").gameObject;
        levelUpSignalGo.SetActive(false);
#endif
    }

    private void Update()
    {
        if (levelUpSignalGo!=null)
        {
            if (hasTower)
            {
                if (towerPersonalProperty.upLevelPrice<=GameController.Instance.coin&&towerLevel<3)
                {
                    levelUpSignalGo.SetActive(true);
                }
                else
                {
                    levelUpSignalGo.SetActive(false);
                }
            }
            else
            {
                if (levelUpSignalGo.activeSelf)
                {
                    levelUpSignalGo.SetActive(false);
                }
            }
        }
    }

    //改回原来样式的Sprite
    private void ChangeSpriteToGrid()
    {
        spriteRenderer.enabled = false;
        spriteRenderer.color = new Color(1, 1, 1, 1);

        if (gridState.canBuild)
        {
            spriteRenderer.sprite = gridSprite;
        }
        else
        {
            spriteRenderer.sprite = cantBuildSprite;
        }
    }

    //初始化格子
    public void InitGrid()
    {
        gridState.canBuild = true;
        gridState.isMonsterPoint = false;
        gridState.hasItem = false;
        spriteRenderer.enabled = true;
        gridState.itemID = -1;
#if Tool
        spriteRenderer.sprite = gridSprite;
        Destroy(currentItem); 
#endif

#if Game
        towerGo = null;
        towerPersonalProperty = null;
        hasTower = false;
#endif

    }

#if Game
    //更新格子状态
    public void UpdateGrid()
    {
        if (gridState.canBuild)
        {
            spriteRenderer.enabled = true;
            if (gridState.hasItem)
            {
                CreateItem();
            }
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

    //创建物品
    private void CreateItem()
    {
        GameObject itemGo = GameController.Instance.GetGameObjectResource(GameController.Instance.mapMaker.bigLevelID.ToString()+"/Item/"+gridState.itemID);
        itemGo.GetComponent<Item>().itemID = gridState.itemID;
        itemGo.transform.SetParent(GameController.Instance.transform);

        Vector3 createPos = transform.position - new Vector3(0,0,3);
        if (gridState.itemID<=2)
        {
            createPos += new Vector3(GameController.Instance.mapMaker.gridWidth,-GameController.Instance.mapMaker.gridHeight)/2;
        }
        else if (gridState.itemID<=4) 
        {
            createPos += new Vector3(GameController.Instance.mapMaker.gridWidth,0)/2;
        }
        itemGo.transform.position = createPos;
        itemGo.GetComponent<Item>().gridPoint = this;
    }

    /// <summary>
    /// 有关格子处理的方法
    /// </summary>

    private void OnMouseDown()
    {
        //选择的是UI则不发生交互
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        GameController.Instance.HandleGrid(this);
    }

    public void ShowGrid()
    {
        if (!hasTower)
        {
            spriteRenderer.enabled = true;
            //显示建塔列表
            towerListGo.transform.position = CorrectTowerListGoPosition();
            towerListGo.SetActive(true);
        }
        else
        {
            handleTowerGanvasGo.transform.position = transform.position;
            CorrectHandleTowerCanvasGoPosition();
            handleTowerGanvasGo.SetActive(true);
            //显示塔的攻击范围
            towerGo.transform.Find("attackRange").gameObject.SetActive(true);
        }
    }

    public void HideGrid()
    {
        if (!hasTower)
        {
            //隐藏建塔列表
            towerListGo.SetActive(false);
        }
        else
        {
            handleTowerGanvasGo.SetActive(false);
            //隐藏塔的范围
            towerGo.transform.Find("attackRange").gameObject.SetActive(false);
        }
        spriteRenderer.enabled = false;
    }
    //显示此格子不能够去建塔
    public void ShowCantBuild()
    {
        spriteRenderer.enabled = true;
        Tween t = DOTween.To(() => spriteRenderer.color, toColor => spriteRenderer.color = toColor, new Color(1, 1, 1, 0), 2f);
        t.OnComplete(() =>
        {
            spriteRenderer.enabled = false;
            spriteRenderer.color = new Color(1, 1, 1, 1);
        });
    }

    //纠正建塔列表的位置
    private Vector3 CorrectTowerListGoPosition()
    {
        Vector3 correctPosition = Vector3.zero;
        if (gridIndex.xIndex<=3&&gridIndex.xIndex>=0)
        {
            correctPosition += new Vector3(GameController.Instance.mapMaker.gridWidth,0,0);
        }
        else if (gridIndex.xIndex <= 11 && gridIndex.xIndex >= 8)
        {
            correctPosition -= new Vector3(GameController.Instance.mapMaker.gridWidth, 0, 0);
        }
        if (gridIndex.yIndex <= 3 && gridIndex.yIndex >= 0)
        {
            correctPosition += new Vector3(0, GameController.Instance.mapMaker.gridHeight, 0);
        }
        else if (gridIndex.yIndex <= 7 && gridIndex.yIndex >= 4)
        {
            correctPosition -= new Vector3(0, GameController.Instance.mapMaker.gridHeight, 0);
        }
        correctPosition += transform.position;
        return correctPosition;
    }

    //纠正操作塔UI画布的方法(纠正按钮位置的方法)
    private void CorrectHandleTowerCanvasGoPosition()
    {
        upLevelButtonTrans.localPosition = Vector3.zero;
        sellTowerButtonTrans.localPosition = Vector3.zero;
        if (gridIndex.yIndex<=0)
        {
            if (gridIndex.xIndex==0)
            {
                sellTowerButtonTrans.position += new Vector3(GameController.Instance.mapMaker.gridWidth*3/4,0,0);
            }
            else
            {
                sellTowerButtonTrans.position -= new Vector3(GameController.Instance.mapMaker.gridWidth*3/4,0,0);
            }
            upLevelButtonTrans.localPosition = upLevelButtonInitPos;
        }
        else if (gridIndex.yIndex>=6)
        {
            if (gridIndex.xIndex==0)
            {
                upLevelButtonTrans.position += new Vector3(GameController.Instance.mapMaker.gridWidth * 3 / 4, 0, 0);
            }
            else
            {
                upLevelButtonTrans.position -= new Vector3(GameController.Instance.mapMaker.gridWidth*3/4,0,0);
            }
            sellTowerButtonTrans.localPosition = sellTowerButtonInitPos;
        }
        else
        {
            upLevelButtonTrans.localPosition = upLevelButtonInitPos;
            sellTowerButtonTrans.localPosition = sellTowerButtonInitPos;
        }
    }

    //建塔后的处理方法
    public void AfterBuild()
    {
        spriteRenderer.enabled = false;
        towerGo = transform.GetChild(1).gameObject;
        tower = towerGo.GetComponent<Tower>();
        towerPersonalProperty = towerGo.GetComponent<TowerPersonalProperty>();
        towerLevel = towerPersonalProperty.towerLevel;
    }

#endif

#if Tool
    private void OnMouseDown()
    {
        //怪物路点
        if (Input.GetKey(KeyCode.P))
        {
            gridState.canBuild = false;
            spriteRenderer.enabled = true;
            gridState.isMonsterPoint = !gridState.isMonsterPoint;
            if (gridState.isMonsterPoint)//是怪物路点
            {
                MapMaker.Instance.monsterPath.Add(gridIndex);
                spriteRenderer.sprite = monsterPathSprite;
            }
            else//不是怪物路点
            {
                MapMaker.Instance.monsterPath.Remove(gridIndex);
                spriteRenderer.sprite = gridSprite;
                gridState.canBuild = true;
            }
        }
        //道具
        else if (Input.GetKey(KeyCode.I))
        {
            gridState.itemID++;
            //当前格子从持有道具状态转化为没有道具
            if (gridState.itemID==itemPrefabs.Length)
            {
                gridState.itemID = -1;
                Destroy(currentItem);
                gridState.hasItem = false;
                return;
            }
            if (currentItem==null)
            {
                //产生道具
                CreateItem();
            }
            else//本身就有道具
            {
                Destroy(currentItem);
                //产生道具
                CreateItem();
            }
            gridState.hasItem = true;
        }


        else if (!gridState.isMonsterPoint)
        {
            gridState.isMonsterPoint = false;
            gridState.canBuild = !gridState.canBuild;
            if (gridState.canBuild)
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }
    }

    //生成道具
    private void CreateItem()
    {
        Vector3 creatPos = transform.position;
        if (gridState.itemID<=2)
        {
            creatPos += new Vector3(MapMaker.Instance.gridWidth,-MapMaker.Instance.gridHeight)/2;
        }
        else if (gridState.itemID<=4)
        {
            creatPos += new Vector3(MapMaker.Instance.gridWidth,0)/2;
        }
        GameObject itemGo = Instantiate(itemPrefabs[gridState.itemID],creatPos,Quaternion.identity);
        currentItem = itemGo;
    }

    //更新格子状态
    public void UpdateGrid()
    {
        if (gridState.canBuild)
        {
            spriteRenderer.sprite = gridSprite;
            spriteRenderer.enabled = true;
            if (gridState.hasItem)
            {
                CreateItem();
            }
        }
        else
        {
            if (gridState.isMonsterPoint)
            {
                spriteRenderer.sprite = monsterPathSprite;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
       
        }
    }

#endif
}
