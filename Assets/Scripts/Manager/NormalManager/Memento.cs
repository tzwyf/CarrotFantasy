using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class Memento
{
    //读取
    public PlayerManager LoadByJson()
    {
        PlayerManager playerManager = new PlayerManager();
        string filePath="";
        if (GameManager.Instance.initPlayerManager) 
        {
            filePath = Application.streamingAssetsPath + "/Json" + "/playerManagerInitData.json";
        }
        else
        {
            filePath = Application.streamingAssetsPath + "/Json" + "/playerManager.json";
        }
        //正常解析
        if (File.Exists(filePath))
        {
            StreamReader sr = new StreamReader(filePath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();
            playerManager = JsonMapper.ToObject<PlayerManager>(jsonStr);
            return playerManager; 
        }
        else
        {
            Debug.Log("PlayerManager读取失败");
        }
        return null;
        //强制解析
        //if (File.Exists(filePath))
        //{
        //    //创建一个StreamReader，用来读取流
        //    StreamReader sr = new StreamReader(filePath);
        //    //将读取到的流赋值给jsonStr
        //    string jsonStr = sr.ReadToEnd();
        //    //关闭
        //    sr.Close();
        //    //将字符串jsonStr转换为Save对象
        //    JsonData jsonData = JsonMapper.ToObject<JsonData>(jsonStr);
        //    playerManager.adventureModelNum = (int)jsonData[0];
        //    playerManager.burriedLevelNum = (int)jsonData[1];
        //    playerManager.bossModelNum = (int)jsonData[2];
        //    playerManager.coin = (int)jsonData[3];
        //    playerManager.killMonsterNum = (int)jsonData[4];
        //    playerManager.killBossNum = (int)jsonData[5];
        //    playerManager.clearItemNum = (int)jsonData[6];

        //    playerManager.UnlockedNormalModelBigLevel = new List<bool>();
        //    JsonData bigLevelJsonData = jsonData[7];
        //    for (int i = 0; i < 3; i++)
        //    {
        //        playerManager.UnlockedNormalModelBigLevel.Add((bool)bigLevelJsonData[i]);
        //    }



        //    playerManager.UnlockedNormalModelLevel = new List<Stage>();
        //    JsonData levelJsonData = jsonData[8];


        //    for (int j = 0; j < 15; j++)//单独的stage
        //    {
        //        JsonData stageJson = levelJsonData[j];
        //        JsonData towerListJson = stageJson[0];
        //        int listLength = (int)stageJson[1];
        //        int[] towerList = new int[listLength];
        //        for (int i = 0; i < listLength; i++)
        //        {
        //            towerList[i] = (int)stageJson[0][i];
        //        }
        //        Stage stage = new Stage((int)stageJson[8], listLength, towerList
        //            , (bool)stageJson[2], (int)stageJson[3], (int)stageJson[4],
        //            (int)stageJson[5], (bool)stageJson[6], (bool)stageJson[7]);
        //        playerManager.UnlockedNormalModelLevel.Add(stage);
        //        Debug.Log(playerManager.UnlockedNormalModelLevel[j].mTowerIDList.Length);
        //    }



        //    playerManager.UnlockedNormalModelLevelNum = new List<int>();
        //    JsonData unLockedLevelNumList = jsonData[9];

        //    for (int i = 0; i < 3; i++)
        //    {
        //        playerManager.UnlockedNormalModelLevelNum.Add((int)unLockedLevelNumList[i]);
        //    }

        //    playerManager.cookies = (int)jsonData[10];
        //    playerManager.milk = (int)jsonData[11];
        //    playerManager.nest = (int)jsonData[12];
        //    playerManager.diamands = (int)jsonData[13];
        //    playerManager.monsterPetDataListLength = (int)jsonData[14];
        //    playerManager.monsterPetDataList = new List<MonsterPetData>();
        //    JsonData monsterPetDataList = jsonData[15];
        //    for (int i = 0; i < playerManager.monsterPetDataListLength; i++)
        //    {
        //        JsonData monsterData = monsterPetDataList[i];
        //        MonsterPetData monsterPetData = new MonsterPetData()
        //        {
        //            monsterLevel = (int)monsterData[0],
        //            remainCookies = (int)monsterData[1],
        //            remainMilk = (int)monsterData[2],
        //            monsterID = (int)monsterData[3]
        //        };
        //        playerManager.monsterPetDataList.Add(monsterPetData);
        //    }
        //    Debug.Log(playerManager.monsterPetDataList.Count);
        //    GameManager.Instance.initPlayerManager = false;
        //    return playerManager;
        //}
    }


    //存贮
    public void SaveByJson()
    {
        PlayerManager playerManager = GameManager.Instance.playerManager;        
        string filePath = Application.streamingAssetsPath + "/Json" + "/playerManager.json";
        string saveJsonStr = JsonMapper.ToJson(playerManager);
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveJsonStr);
        sw.Close();
    }
}
