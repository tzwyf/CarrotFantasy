using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 游戏场景UI面板
/// </summary>
public class NormalModelPanel : BasePanel {

    //控制的页面
    private GameObject topPageGo;
    private GameObject gameOverPageGo;
    private GameObject gameWinPageGo;
    private GameObject menuPageGo;
    private GameObject img_FinalWave;
    private GameObject img_StartGame;
    private GameObject prizePageGo;

    public int totalRound;

    //引用
    public TopPage topPage;

    protected override void Awake()
    {
        base.Awake();
        transform.SetSiblingIndex(1);
        topPageGo = transform.Find("Img_TopPage").gameObject;
        gameOverPageGo = transform.Find("GameOverPage").gameObject;
        gameWinPageGo = transform.Find("GameWinPage").gameObject;
        menuPageGo = transform.Find("MenuPage").gameObject;
        prizePageGo = transform.Find("PrizePage").gameObject;
        img_FinalWave = transform.Find("Img_FinalWave").gameObject;
        img_StartGame = transform.Find("StartUI").gameObject;
        topPage = topPageGo.GetComponent<TopPage>();
    }

    private void OnEnable()
    {     
        InvokeRepeating("PlayAudio",0.5f,1);
        Invoke("StartGame", 3.5f);
    }

    //播放开始游戏倒计时声音
    private void PlayAudio()
    {
        img_StartGame.SetActive(true);
        GameController.Instance.PlayEffectMusic("NormalMordel/CountDown");
    }

    //开始游戏
    private void StartGame()
    {
        GameController.Instance.PlayEffectMusic("NormalMordel/Go");
#if Game
        GameController.Instance.StartGame();
#endif
        img_StartGame.SetActive(false); 
        CancelInvoke();
    }

    /// <summary>
    /// 与面板处理有关的方法
    /// </summary>
    public override void EnterPanel() 
    {
        base.EnterPanel();
        totalRound = GameController.Instance.currentStage.mTotalRound;
        topPageGo.SetActive(true);
    }

    public override void UpdatePanel()
    {
        base.UpdatePanel();
        topPage.UpdateRoundText();
        topPage.UpdateCoinText();
    }

    /// <summary>
    /// 页面显示隐藏的方法
    /// </summary>
    
    //奖励页面
    public void ShowPrizePage()
    {
        GameController.Instance.isPause = true;
        prizePageGo.SetActive(true);
    }

    public void ClosePrizePage()
    {
        mUIFacade.PlayButtonAudioClip();
        GameController.Instance.isPause = false;
        prizePageGo.SetActive(false);
    }

    //菜单页面
    public void ShowMenuPage()
    {
        mUIFacade.PlayButtonAudioClip();
        GameController.Instance.isPause = true;
        menuPageGo.SetActive(true);
    }

    public void CloseMenuPage()
    {
        GameController.Instance.isPause = false;
        menuPageGo.SetActive(false);
    }

    //胜利页面
    public void ShowGameWinPage()
    {
        Stage stage = GameManager.Instance.playerManager.unLockedNormalModelLevelList[GameController.Instance.currentStage.mLevelID-1+(GameController.Instance.currentStage.mBigLevelID-1)*5];
        //道具徽章更新
        if (GameController.Instance.IfAllClear())
        {
            stage.mAllClear = true;
        }
        //萝卜徽章更新
        int carrotState = GameController.Instance.GetCarrotState();
        if (carrotState!=0&&stage.mCarrotState!=0)
        {
            if (stage.mCarrotState>carrotState)
            {
                stage.mCarrotState = carrotState;
            }
        }
        else if (stage.mCarrotState == 0)
        {
            stage.mCarrotState = carrotState;
        }
        //解锁下一个关卡
        //不是最后一关且不是隐藏关卡才能解锁下一关
        if (GameController.Instance.currentStage.mLevelID%5!=0&& (GameController.Instance.currentStage.mLevelID - 1 + (GameController.Instance.currentStage.mBigLevelID - 1) * 5)<GameManager.Instance.playerManager.unLockedNormalModelLevelList.Count)
        {
            GameManager.Instance.playerManager.unLockedNormalModelLevelList[GameController.Instance.currentStage.mLevelID + (GameController.Instance.currentStage.mBigLevelID - 1) * 5].unLocked = true;
        }
        UpdatePlayerManagerData();
        gameWinPageGo.SetActive(true);
        GameController.Instance.gameOver = false;
        GameManager.Instance.playerManager.adventrueModelNum++;
        GameController.Instance.PlayEffectMusic("NormalMordel/Perfect");
    }

    //失败页面
    public void ShowGameOverPage()
    {
        UpdatePlayerManagerData();
        gameOverPageGo.SetActive(true);
        GameController.Instance.gameOver = false;
        GameController.Instance.PlayEffectMusic("NormalMordel/Lose");
    }

    /// <summary>
    /// 与UI显示有关的方法
    /// </summary>

    //更新回合的显示文本
    public void ShowRoundText(Text roundText)
    {
        int roundNum = GameController.Instance.level.currentRound + 1;
        string roundStr = "";
        if (roundNum<10)
        {
            roundStr = "0  " + roundNum.ToString();
        }
        else
        {
            roundStr = (roundNum / 10).ToString() + "  " + (roundNum % 10).ToString();
        }
        roundText.text = roundStr;
    }

    //最后一波怪UI
    public void ShowFinalWaveUI()
    {
        GameController.Instance.PlayEffectMusic("NormalMordel/Finalwave");
        img_FinalWave.SetActive(true);
        Invoke("CloseFinalWaveUI",0.508f);
    }

    private void CloseFinalWaveUI()
    {
        img_FinalWave.SetActive(false);
        GameController.Instance.level.HandleLastRound();
    }

    /// <summary>
    /// 与关卡处理有关的方法
    /// </summary>

    //更新基础数据
    private void UpdatePlayerManagerData()
    {
        GameManager.Instance.playerManager.coin += GameController.Instance.coin;
        GameManager.Instance.playerManager.killMonsterNum += GameController.Instance.killMonsterTotalNum;
        GameManager.Instance.playerManager.clearItemNum += GameController.Instance.clearItemNum;
    }
    

    //重玩
    public void Replay()
    {
        mUIFacade.PlayButtonAudioClip();
        UpdatePlayerManagerData();
        mUIFacade.ChangeSceneState(new NormalModelSceneState(mUIFacade));
        Invoke("ResetGame",2);
    }

    //重置当前关卡游戏
    private void ResetGame()
    {
        //GameController.Instance.gameOver = false;
        SceneManager.LoadScene(3);
        ResetUI();
        gameObject.SetActive(true);
    }

    //重置页面UI显示状态
    public void ResetUI()
    {
        gameOverPageGo.SetActive(false);
        gameWinPageGo.SetActive(false);
        menuPageGo.SetActive(false);
        gameObject.SetActive(false);
    }

    //选择其他关卡
    public void ChooseOtherLevel()
    {
        mUIFacade.PlayButtonAudioClip();
        GameController.Instance.gameOver = false;
        UpdatePlayerManagerData();
        Invoke("ToOtherScene",2);
        mUIFacade.ChangeSceneState(new NormalGameOptionSceneState(mUIFacade));
    }

    //返回关卡选择场景
    public void ToOtherScene()
    {
        GameController.Instance.gameOver = false;
        ResetUI();
        SceneManager.LoadScene(2);
    }
}
