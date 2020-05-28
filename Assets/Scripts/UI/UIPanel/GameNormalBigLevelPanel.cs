using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 大关卡选择面板
/// </summary>
public class GameNormalBigLevelPanel : BasePanel {

    public Transform bigLevelContentTrans;//滚动视图的content
    public int bigLevelPageCount;//大关卡总数
    private SlideScrollView slideScrollView;
    private PlayerManager playerManager;
    private Transform[] bigLevelPage;//大关卡按钮数组

    private bool hasRigisterEvent;

    protected override void Awake()
    {
        base.Awake();
        playerManager = mUIFacade.mPlayerManager;
        bigLevelPage = new Transform[bigLevelPageCount];
        slideScrollView = transform.Find("Scroll View").GetComponent<SlideScrollView>();
        //显示大关卡信息
        for (int i = 0; i < bigLevelPageCount; i++)
        {
            bigLevelPage[i] = bigLevelContentTrans.GetChild(i);
            ShowBigLevelState(playerManager.unLockedNormalModelBigLevelList[i], playerManager.unLockedeNormalModelLevelNum[i], 5, bigLevelPage[i], i + 1);
        }
        hasRigisterEvent = true;
    }

    private void OnEnable()
    {
        for (int i = 0; i < bigLevelPageCount; i++)
        {
            bigLevelPage[i] = bigLevelContentTrans.GetChild(i);
            ShowBigLevelState(playerManager.unLockedNormalModelBigLevelList[i], playerManager.unLockedeNormalModelLevelNum[i], 5, bigLevelPage[i], i + 1);
        }
    }


    //进入退出面板
    public override void EnterPanel()
    {
        base.EnterPanel();
        slideScrollView.Init();
        gameObject.SetActive(true);
    }

    public override void ExitPanel()
    {
        base.ExitPanel();
        gameObject.SetActive(false);
    }

    //显示大关卡信息
    public void ShowBigLevelState(bool unLocked,int unLockedLevelNum,int totalNum,Transform theBigLevelButtonTrans,int bigLevelID)
    {
        if (unLocked)//解锁状态
        {
            theBigLevelButtonTrans.Find("Img_Lock").gameObject.SetActive(false);
            theBigLevelButtonTrans.Find("Img_Page").gameObject.SetActive(true);
            theBigLevelButtonTrans.Find("Img_Page").Find("Tex_Page").GetComponent<Text>().text
                = unLockedLevelNum.ToString() + "/" + totalNum.ToString();
            Button theBigLevelButtonCom = theBigLevelButtonTrans.GetComponent<Button>();
            theBigLevelButtonCom.interactable = true;
            if (!hasRigisterEvent)
            {
                theBigLevelButtonCom.onClick.AddListener(() =>
                {
                    mUIFacade.PlayButtonAudioClip();
                    //离开大关卡页面
                    mUIFacade.currentScenePanelDict[StringManager.GameNormalBigLevelPanel].ExitPanel();
                    //进入小关卡
                    GameNormalLevelPanel gameNormalLevelPanel = mUIFacade.currentScenePanelDict[StringManager.GameNormalLevelPanel] as GameNormalLevelPanel;
                    gameNormalLevelPanel.ToThisPanel(bigLevelID);
                    //设置所在页面
                    GameNormalOptionPanel gameNormalOptionPanel = mUIFacade.currentScenePanelDict[StringManager.GameNormalOptionPanel] as GameNormalOptionPanel;
                    gameNormalOptionPanel.isInBigLevelPanel = false;
                });
            }
           
        }
        else//未解锁
        {
            theBigLevelButtonTrans.Find("Img_Lock").gameObject.SetActive(true);
            theBigLevelButtonTrans.Find("Img_Page").gameObject.SetActive(false);
            theBigLevelButtonTrans.GetComponent<Button>().interactable = false;
        }
    }


    //翻页按钮方法
    public void ToNextPage()
    {
        mUIFacade.PlayButtonAudioClip();
        slideScrollView.ToNextPage(); 
    }

    public void ToLastPage()
    {
        mUIFacade.PlayButtonAudioClip();
        slideScrollView.ToLastPage();
    }
}
