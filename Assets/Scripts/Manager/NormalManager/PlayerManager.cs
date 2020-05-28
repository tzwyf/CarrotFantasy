using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家的管理，负责保存以及加载各种玩家以及游戏的信息
/// </summary>
public class PlayerManager
{
    public int adventrueModelNum; //冒险模式解锁的地图个数
    public int burriedLevelNum; //隐藏关卡解锁的地图个数
    public int bossModelNum;//boss模式KO的BOSS
    public int coin;//获得金币的总数
    public int killMonsterNum;//杀怪总数
    public int killBossNum;//杀掉BOSS的总数
    public int clearItemNum;//清理道具的总数
    public List<bool> unLockedNormalModelBigLevelList;//大关卡
    public List<Stage> unLockedNormalModelLevelList;//所有的小关卡
    public List<int> unLockedeNormalModelLevelNum;//解锁小关卡数量

    //怪物窝
    public int cookies;
    public int milk;
    public int nest;
    public int diamands;
    public List<MonsterPetData> monsterPetDataList;//宠物喂养信息

    //用于玩家初始Json文件的制作
    //public PlayerManager()
    //{
    //    adventrueModelNum = 0;
    //    burriedLevelNum = 0;
    //    bossModelNum = 0;
    //    coin = 0;
    //    killMonsterNum = 0;
    //    killBossNum = 0;
    //    clearItemNum = 0;
    //    cookies = 100;
    //    milk = 100;
    //    nest = 1;
    //    diamands = 10;
    //    unLockedeNormalModelLevelNum = new List<int>()
    //    {
    //        1,0,0
    //    };
    //    unLockedNormalModelBigLevelList = new List<bool>()
    //    {
    //        true,false,false
    //    };
    //    unLockedNormalModelLevelList = new List<Stage>()
    //    {
    //           new Stage(10,1,new int[]{ 1},false,0,1,1,true,false),
    //           new Stage(9,1,new int[]{ 2},false,0,2,1,false,false),
    //           new Stage(8,2,new int[]{ 1,2},false,0,3,1,false,false),
    //           new Stage(10,1,new int[]{ 3},false,0,4,1,false,false),
    //           new Stage(9,3,new int[]{ 1,2,3},false,0,5,1,false,true),
    //           new Stage(8,2,new int[]{ 2,3},false,0,1,2,false,false),
    //           new Stage(10,2,new int[]{ 1,3},false,0,2,2,false,false),
    //           new Stage(9,1,new int[]{ 4},false,0,3,2,false,false),
    //           new Stage(8,2,new int[]{ 1,4},false,0,4,2,false,false),
    //           new Stage(10,2,new int[]{ 2,4},false,0,5,2,false,true),
    //           new Stage(9,2,new int[]{ 3,4},false,0,1,3,false,false),
    //           new Stage(8,1,new int[]{ 5},false,0,2,3,false,false),
    //           new Stage(7,2,new int[]{ 4,5},false,0,3,3,false,false),
    //           new Stage(10,3,new int[]{ 1,3,5},false,0,4,3,false,false),
    //           new Stage(10,3,new int[]{ 1,4,5},false,0,5,3,false,true)
    //    };
    //    monsterPetDataList = new List<MonsterPetData>()
    //    {
    //        new MonsterPetData()
    //        {
    //            monsterID=1,
    //            monsterLevel=1,
    //            remainCookies=0,
    //            remainMilk=0
    //        }, 

    //    };
    //}

    //用于玩家所有关卡都解锁的Json文件的制作
    //public PlayerManager()
    //{
    //    adventrueModelNum = 12;
    //    burriedLevelNum = 3;
    //    bossModelNum = 0;
    //    coin = 999;
    //    killMonsterNum = 999;
    //    killBossNum = 0;
    //    clearItemNum = 999;
    //    cookies = 1000;
    //    milk = 1000;
    //    nest = 10;
    //    diamands = 1000;
    //    unLockedeNormalModelLevelNum = new List<int>()
    //    {
    //        5,5,5
    //    };
    //    unLockedNormalModelBigLevelList = new List<bool>()
    //    {
    //        true,true,true
    //    };
    //    unLockedNormalModelLevelList = new List<Stage>()
    //    {
    //           new Stage(10,1,new int[]{ 1},false,0,1,1,true,false),
    //           new Stage(9,1,new int[]{ 2},false,0,2,1,true,false),
    //           new Stage(8,2,new int[]{ 1,2},false,0,3,1,true,false),
    //           new Stage(10,1,new int[]{ 3},false,0,4,1,true,false),
    //           new Stage(9,3,new int[]{ 1,2,3},false,0,5,1,false,true),
    //           new Stage(8,2,new int[]{ 2,3},false,0,1,2,true,false),
    //           new Stage(10,2,new int[]{ 1,3},false,0,2,2,true,false),
    //           new Stage(9,1,new int[]{ 4},false,0,3,2,true,false),
    //           new Stage(8,2,new int[]{ 1,4},false,0,4,2,true,false),
    //           new Stage(10,2,new int[]{ 2,4},false,0,5,2,false,true),
    //           new Stage(9,2,new int[]{ 3,4},false,0,1,3,true,false),
    //           new Stage(8,1,new int[]{ 5},false,0,2,3,true,false),
    //           new Stage(7,2,new int[]{ 4,5},false,0,3,3,true,false),
    //           new Stage(10,3,new int[]{ 1,3,5},false,0,4,3,true,false),
    //           new Stage(10,3,new int[]{ 1,4,5},false,0,5,3,false,true)
    //    };
    //    monsterPetDataList = new List<MonsterPetData>()
    //    {
    //        new MonsterPetData()
    //        {
    //            monsterID=1,
    //            monsterLevel=1,
    //            remainCookies=0,
    //            remainMilk=0
    //        },
    //        new MonsterPetData()
    //        {
    //            monsterID=2,
    //            monsterLevel=1,
    //            remainCookies=0,
    //            remainMilk=0
    //        },
    //        new MonsterPetData()
    //        {
    //            monsterID=3,
    //            monsterLevel=1,
    //            remainCookies=0,
    //            remainMilk=0
    //        }
    //    };
    //}

    public void SaveData()
    {
        Memento memento = new Memento();
        memento.SaveByJson();
    }

    public void ReadData()
    {
        Memento memento = new Memento();
        PlayerManager playerManager = memento.LoadByJson();
        //数据信息
        adventrueModelNum = playerManager.adventrueModelNum;
        burriedLevelNum = playerManager.burriedLevelNum;
        bossModelNum = playerManager.bossModelNum;
        coin = playerManager.coin;
        killMonsterNum = playerManager.killMonsterNum;
        killBossNum = playerManager.killBossNum;
        clearItemNum = playerManager.clearItemNum;
        cookies = playerManager.cookies;
        milk = playerManager.milk;
        nest = playerManager.nest;
        diamands = playerManager.diamands;
        //列表
        unLockedNormalModelBigLevelList = playerManager.unLockedNormalModelBigLevelList;
        unLockedNormalModelLevelList = playerManager.unLockedNormalModelLevelList;
        unLockedeNormalModelLevelNum = playerManager.unLockedeNormalModelLevelNum;
        monsterPetDataList = playerManager.monsterPetDataList;
    }
}
