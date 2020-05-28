using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏控制管理，负责控制游戏的整个逻辑
/// </summary>
public class GameController : MonoBehaviour {

    private static GameController _instance;
    public static GameController Instance
    {
        get
        {
            return _instance;
        }
    }

    //引用
    public Level level;
    private GameManager mGameManager;
    public int[] mMonsterIDList;//当前波次的产怪列表
    public Stage currentStage;
    public MapMaker mapMaker;
    public Transform targetTrans;//集火目标
    public GameObject targetSignal;//集火信号
    public GridPoint selectGrid;//上一个选择的格子

    //游戏资源
    public RuntimeAnimatorController[] controllers;//怪物的动画播放控制器

    //游戏UI的面板
    public NormalModelPanel normalModelpanel;

    //用于计数的成员变量
    public int killMonsterNum;//当前波次杀怪数
    public int clearItemNum;//道具销毁数量
    public int killMonsterTotalNum;//杀怪总数
    public int mMonsterIDIndex;//用于统计当前怪物列表产生怪物的索引

    //属性值
    public int carrotHp = 10;
    public int coin;
    public int gameSpeed;//当前游戏进行速度
    public bool isPause;
    public bool creatingMonster;//是否继续产怪
    public bool gameOver;//游戏是否结束

    //建造者
    public MonsterBuilder monsterBuilder;
    public TowerBuilder towerBuilder;

    //建塔有关的成员变量
    public Dictionary<int, int> towerPriceDict;//建塔价格表  
    public GameObject towerListGo;//建塔按钮列表
    public GameObject handleTowerCanvasGo;//处理塔升级与买卖的画布



    private void Awake()
    {
#if Game
        _instance = this;
        mGameManager = GameManager.Instance;
        //测试代码
        //currentStage = new Stage(10,5,new int[] { 1,2,3,4,5},false,0,1,1,true,false);
        currentStage = mGameManager.currentStage;
        normalModelpanel = mGameManager.uiManager.mUIFacade.currentScenePanelDict[StringManager.NormalModelPanel] as NormalModelPanel;
        normalModelpanel.EnterPanel();
        mapMaker = GetComponent<MapMaker>();
        mapMaker.InitMapMaker(); 
        mapMaker.LoadMap(currentStage.mBigLevelID,currentStage.mLevelID);
        //成员变量赋值
        gameSpeed = 1;
        coin = 1000;

        monsterBuilder = new MonsterBuilder();
        towerBuilder = new TowerBuilder();
        //建塔列表的处理
        for (int i = 0; i < currentStage.mTowerIDList.Length; i++)
        {
            GameObject itemGo = mGameManager.GetGameObjectResource(FactoryType.UIFactory,"Btn_TowerBuild");
            itemGo.transform.GetComponent<ButtonTower>().towerID = currentStage.mTowerIDList[i];
            itemGo.transform.SetParent(towerListGo.transform);
            itemGo.transform.localPosition = Vector3.zero;
            itemGo.transform.localScale = Vector3.one;
        }
        //建塔价格表
        towerPriceDict = new Dictionary<int, int>
        {
            { 1,100},
            { 2,120},
            { 3,140},
            { 4,160},
            { 5,160}
        };

        controllers = new RuntimeAnimatorController[12];
        for (int i = 0; i < controllers.Length; i++)
        {
            controllers[i] = GetRuntimeAnimatorController("Monster/"+mapMaker.bigLevelID.ToString()+"/"+(i+1).ToString());
        }
        level = new Level(mapMaker.roundInfoList.Count, mapMaker.roundInfoList);
        normalModelpanel.topPage.UpdateCoinText();
        normalModelpanel.topPage.UpdateRoundText();
        //level.HandleRound();
        isPause = true;
#endif
    }

    private void Update()
    {
#if Game
        if (!isPause)
        {
            //产怪逻辑
            if (killMonsterNum>=mMonsterIDList.Length)
            {
                //添加当前回合数的索引
                if (level.currentRound==level.totalRound)
                {
                    return;
                }
                AddRoundNum();
            }
            else
            {
                if (!creatingMonster)
                {
                    CreateMonster();
                }
            }
        }
        else
        {
            //暂停
            StopCreateMonster();
            creatingMonster = false;
        }
#endif
    }

    /// <summary>
    /// 与集火目标有关的方法
    /// </summary>
    public void ShowSignal()
    {
        PlayEffectMusic("NormalMordel/Tower/ShootSelect");
        targetSignal.transform.position = targetTrans.position + new Vector3(0,mapMaker.gridHeight/2,0);
        targetSignal.transform.SetParent(targetTrans);
        targetSignal.SetActive(true);
    }

    public void HideSignal()
    {
        targetSignal.gameObject.SetActive(false);
        targetTrans = null;
    }

    /// <summary>
    /// 与玩家信息有关的方法
    /// </summary>
    
    //改变玩家金币
    public void ChangeCoin(int coinNum)
    {
        coin += coinNum;
        //更新游戏UI显示
        normalModelpanel.UpdatePanel();
    }

    //萝卜血量减少
    public void DecreaseHP()
    {
        PlayEffectMusic("NormalMordel/Carrot/Crash");
        carrotHp--;
        //更新萝卜UI显示
        mapMaker.carrot.UpdateCarrotUI();
    }

    //判断当前道具是否全部清除
    public bool IfAllClear()
    {
        for (int x = 0; x < MapMaker.xColumn; x++)
        {
            for (int y = 0; y < MapMaker.yRow; y++)
            {
                if (mapMaker.gridPoints[x,y].gridState.hasItem)
                {
                    return false;
                }
            }
        }
        return true;
    }

    //获取萝卜状态
    public int GetCarrotState()
    {
        int carrotState = 0;
        if (carrotHp>=4)
        {
            carrotState = 1;
        }
        else if (carrotHp>=2)
        {
            carrotState = 2;
        }
        else
        {
            carrotState = 3;
        }
        return carrotState;
    }

    /// <summary>
    /// 产生怪物的有关方法
    /// </summary>
    //产怪方法
    public void CreateMonster()
    {
        creatingMonster = true;
        InvokeRepeating("InstantiateMonster",(float)1/gameSpeed, (float)1 / gameSpeed);
    }

    //具体产怪方法
    private void InstantiateMonster()
    {
        PlayEffectMusic("NormalMordel/Monster/Create");
        if (mMonsterIDIndex >= mMonsterIDList.Length)
        {
            StopCreateMonster();
            return;
        }
        //产生特效
        GameObject effectGo = GetGameObjectResource("CreateEffect");
        effectGo.transform.SetParent(transform);
        effectGo.transform.position = mapMaker.monsterPathPos[0];
        //产生怪物
        if (mMonsterIDIndex<mMonsterIDList.Length)
        {
            monsterBuilder.m_monsterID = level.roundList[level.currentRound].roundInfo.mMonsterIDList[mMonsterIDIndex];
        }

        GameObject monsterGo = monsterBuilder.GetProduct();
        monsterGo.transform.SetParent(transform);
        monsterGo.transform.position = mapMaker.monsterPathPos[0];
        mMonsterIDIndex++;
    }
    //停止产生
    public void StopCreateMonster()
    {
        CancelInvoke();
    }
    //增加当前回合数，并且交给下一个回合来处理产怪
    public void AddRoundNum()
    {
        mMonsterIDIndex = 0;
        killMonsterNum = 0;
        level.AddRoundNum();
        level.HandleRound();
        //更新有关回合显示的UI
        normalModelpanel.UpdatePanel();
    }

    /// <summary>
    /// 与游戏处理逻辑有关的方法
    /// </summary>

#if Game

    //打开奖品页面
    public void ShowPrizePage()
    {
        normalModelpanel.ShowPrizePage();
    }

    //开始游戏
    public void StartGame()
    {
        isPause = false;
        level.HandleRound();
    }

    //格子处理方法
    public void HandleGrid(GridPoint grid)//当前选择的格子
    {
        if (grid.gridState.canBuild)
        {
            if (selectGrid==null)//没有上一个格子
            {
                selectGrid = grid;
                selectGrid.ShowGrid();
                PlayEffectMusic("NormalMordel/Grid/GridSelect");
            }
            else if (grid==selectGrid)//选中同一个格子
            {
                grid.HideGrid();
                selectGrid = null;
                PlayEffectMusic("NormalMordel/Grid/GridDeselect");
            }
            else if (grid!=selectGrid)//选中不同格子
            {
                selectGrid.HideGrid();
                selectGrid = grid;
                selectGrid.ShowGrid();
                PlayEffectMusic("NormalMordel/Grid/GridSelect");
            }
        }
        else
        {
            grid.HideGrid();
            grid.ShowCantBuild();
            PlayEffectMusic("NormalMordel/Grid/SelectFault");
            if (selectGrid!=null)
            {
                selectGrid.HideGrid();
            }
        }
    }
#endif
    /// <summary>
    /// 资源获取的有关方法
    /// </summary>
    /// <param name="resourcePath"></param>
    /// <returns></returns>
    public Sprite GetSprite(string resourcePath)
    {
        return mGameManager.GetSprite(resourcePath);
    }
    public AudioClip GetAudioClip(string resourcePath)
    {
        return mGameManager.GetAudioClip(resourcePath);
    }
    public RuntimeAnimatorController GetRuntimeAnimatorController(string resourcePath)
    {
        return mGameManager.GetRunTimeAnimatorController(resourcePath);
    }
    public GameObject GetGameObjectResource(string resourcePath)
    {
        return mGameManager.GetGameObjectResource(FactoryType.GameFactory,resourcePath);
    }
    public void PushGameObjectToFactory(string resourcePath,GameObject itemGo)
    {
        mGameManager.PushGameObjectToFactory(FactoryType.GameFactory,resourcePath,itemGo);
    }

    //播放特效音
    public void PlayEffectMusic(string audioClipPath)
    {
        mGameManager.audioSourceManager.PlayEffectMusic(GetAudioClip(audioClipPath));
    }
}
