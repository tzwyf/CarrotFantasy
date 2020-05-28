using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

/// <summary>
/// 地图编辑器工具，游戏中作为地图加载产生工具
/// </summary>
public class MapMaker : MonoBehaviour {

#if Tool
    //是否画线
    public bool drawLine;
    public GameObject gridGo;
     private static MapMaker _instance;

    public static MapMaker Instance
    {
        get
        {
            return _instance;
        }
    }
#endif
    //地图的有关属性
    //地图
    private float mapWidth;
    private float mapHeight;
    //格子
    [HideInInspector]
    public float gridWidth;
    [HideInInspector]
    public float gridHeight;
    //当前关卡索引
    public int bigLevelID;
    public int levelID;
    //全部的格子对象
    public GridPoint[,] gridPoints;

    //行列
    public const int yRow = 8;
    public const int xColumn = 12;

    //怪物路径点
    [HideInInspector]
    public List<GridPoint.GridIndex> monsterPath;
    //怪物路径点的具体位置
    [HideInInspector]
    public List<Vector3> monsterPathPos;

    private SpriteRenderer bgSR;
    private SpriteRenderer roadSR;

    //每一波次产生的怪物ID列表
    public List<Round.RoundInfo> roundInfoList;

    [HideInInspector]
    public Carrot carrot;

    private void Awake()
    {
#if Tool
        _instance = this;
        InitMapMaker();
#endif

    }

    //初始化地图
    public void InitMapMaker()
    {
        CalculateSize();
        gridPoints = new GridPoint[xColumn, yRow];
        monsterPath = new List<GridPoint.GridIndex>();
        for (int x = 0; x < xColumn; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
#if Tool
                GameObject itemGo = Instantiate(gridGo,transform.position,transform.rotation);
#endif
#if Game
                GameObject itemGo = GameController.Instance.GetGameObjectResource("Grid");
#endif 
                itemGo.transform.position = CorretPostion(x * gridWidth, y * gridHeight);
                itemGo.transform.SetParent(transform);
                gridPoints[x, y] = itemGo.GetComponent<GridPoint>();
                gridPoints[x, y].gridIndex.xIndex = x;
                gridPoints[x, y].gridIndex.yIndex = y;
            }
        }
        bgSR = transform.Find("BG").GetComponent<SpriteRenderer>();
        roadSR = transform.Find("Road").GetComponent<SpriteRenderer>();

    }

#if Game
    //加载地图
    public void LoadMap(int bigLevel,int level)
    {
        bigLevelID = bigLevel;
        levelID = level;
        LoadLevelFile(LoadLevelInfoFile("Level"+bigLevelID.ToString()+"_"+levelID.ToString()+".json"));
        monsterPathPos = new List<Vector3>();
        for (int i = 0; i < monsterPath.Count; i++)
        {
            monsterPathPos.Add(gridPoints[monsterPath[i].xIndex,monsterPath[i].yIndex].transform.position);
        }

        //起始点与终止点
        GameObject startPointGo = GameController.Instance.GetGameObjectResource("startPoint");
        startPointGo.transform.position = monsterPathPos[0];
        startPointGo.transform.SetParent(transform);

        GameObject endPointGo = GameController.Instance.GetGameObjectResource("Carrot");
        endPointGo.transform.position = monsterPathPos[monsterPathPos.Count-1]-new Vector3(0,0,1);
        endPointGo.transform.SetParent(transform);
        carrot = endPointGo.GetComponent<Carrot>();
    }

#endif
    //纠正位置
    public Vector3 CorretPostion(float x,float y)
    {
        return new Vector3(x-mapWidth/2+gridWidth/2,y-mapHeight/2+gridHeight/2);
    }

    //计算地图格子宽高
    private void CalculateSize()
    {
        Vector3 leftDown = new Vector3(0, 0);
        Vector3 rightUp = new Vector3(1, 1);

        Vector3 posOne = Camera.main.ViewportToWorldPoint(leftDown);
        Vector3 posTwo = Camera.main.ViewportToWorldPoint(rightUp);

        mapWidth = posTwo.x - posOne.x;
        mapHeight = posTwo.y - posOne.y;

        gridWidth = mapWidth / xColumn;
        gridHeight = mapHeight / yRow;

    }

#if Tool
    //画格子用于辅助设计
    private void OnDrawGizmos()
    {
        if (drawLine)
        {
            CalculateSize();
            Gizmos.color = Color.green;

            //画行
            for (int y = 0; y <= yRow; y++)
            {
                Vector3 startPos = new Vector3(-mapWidth/2,-mapHeight/2+y*gridHeight);
                Vector3 endPos = new Vector3(mapWidth / 2, -mapHeight / 2 + y * gridHeight);
                Gizmos.DrawLine(startPos,endPos);
            }
            //画列
            for (int x = 0; x <= xColumn; x++)
            {
                Vector3 startPos = new Vector3(-mapWidth / 2 + gridWidth * x, mapHeight / 2);
                Vector3 endPos = new Vector3(-mapWidth / 2 + x * gridWidth, -mapHeight / 2);
                Gizmos.DrawLine(startPos, endPos);
            }
        }
    }
#endif
/// <summary>
/// 有关地图编辑的方法
/// </summary>

    //清除怪物路点
    public void ClearMonsterPath()
    {
        monsterPath.Clear();
    }

    //恢复地图编辑默认状态
    public void RecoverTowerPoint()
    {
        ClearMonsterPath();
        for (int x = 0; x < xColumn; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                gridPoints[x, y].InitGrid();
            }
        }
    }

    //初始化地图
    public void InitMap()
    {
        bigLevelID = 0;
        levelID = 0;
        RecoverTowerPoint();
        roundInfoList.Clear();
        bgSR.sprite = null;
        roadSR.sprite = null;
    }
#if Tool
    //生成LevelInfo对象用来保存文件
    private LevelInfo CreateLevelInfoGo()
    {
        LevelInfo levelInfo = new LevelInfo
        {
            bigLevelID = this.bigLevelID,
            levelID = this.levelID
        };
        levelInfo.gridPoints = new List<GridPoint.GridState>();      
        for (int x = 0; x < xColumn; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                levelInfo.gridPoints.Add(gridPoints[x, y].gridState);
            }
        }
        levelInfo.monsterPath = new List<GridPoint.GridIndex>();
        for (int i = 0; i < monsterPath.Count; i++)
        {
            levelInfo.monsterPath.Add(monsterPath[i]);
        }
        levelInfo.roundInfo = new List<Round.RoundInfo>();
        for (int i = 0; i < roundInfoList.Count; i++)
        {
            levelInfo.roundInfo.Add(roundInfoList[i]);
        }
        Debug.Log("保存成功");
        return levelInfo;
    }
    //保存当前关卡的数据文件
    public void SaveLevelFileByJson()
    {
        LevelInfo levelInfoGo = CreateLevelInfoGo();
        string filePath = Application.streamingAssetsPath + "/Json/Level/" + "Level" + bigLevelID.ToString() + "_" + levelID.ToString() + ".json";
        string saveJsonStr = JsonMapper.ToJson(levelInfoGo);
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveJsonStr);
        sw.Close();
    }
#endif
    //读取关卡文件解析json转化为LevelInfo对象
    public LevelInfo LoadLevelInfoFile(string fileName)
    {
        LevelInfo levelInfo = new LevelInfo();
        string filePath=Application.streamingAssetsPath+ "/Json/Level/" + fileName;
        if (File.Exists(filePath))
        {
            StreamReader sr = new StreamReader(filePath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();
            levelInfo = JsonMapper.ToObject<LevelInfo>(jsonStr);
            return levelInfo;
        }
        Debug.Log("文件加载失败，加载路径是"+filePath);
        return null;
    }

    public void LoadLevelFile(LevelInfo levelInfo)
    {
        bigLevelID = levelInfo.bigLevelID;
        levelID = levelInfo.levelID;
        for (int x = 0; x < xColumn; x++)
        {
            for (int y = 0; y < yRow; y++)
            {
                gridPoints[x, y].gridState = levelInfo.gridPoints[y+x*yRow];
                //更新格子的状态
                gridPoints[x, y].UpdateGrid();
            }
        }
        monsterPath.Clear();
        for (int i = 0; i < levelInfo.monsterPath.Count; i++)
        {
            monsterPath.Add(levelInfo.monsterPath[i]);
        }
        roundInfoList = new List<Round.RoundInfo>();
        for (int i = 0; i < levelInfo.roundInfo.Count; i++)
        {
            roundInfoList.Add(levelInfo.roundInfo[i]);
        }
        bgSR.sprite = Resources.Load<Sprite>("Pictures/NormalMordel/Game/"+bigLevelID.ToString()+"/"+"BG"+(levelID/3).ToString());
        roadSR.sprite= Resources.Load<Sprite>("Pictures/NormalMordel/Game/" + bigLevelID.ToString() + "/" + "Road" + levelID);
    }
}
