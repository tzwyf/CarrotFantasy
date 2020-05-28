using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 关卡选择共用面板
/// </summary>
public class GameNormalOptionPanel : BasePanel {

    [HideInInspector]
    public bool isInBigLevelPanel = true;

    public void ReturnToLastPanel()
    {
        if (isInBigLevelPanel)
        {
            //返回主界面
            mUIFacade.ChangeSceneState(new MainSceneState(mUIFacade));
        }
        else
        {
            //返回大关卡选择面板
            mUIFacade.currentScenePanelDict[StringManager.GameNormalLevelPanel].ExitPanel();
            mUIFacade.currentScenePanelDict[StringManager.GameNormalBigLevelPanel].EnterPanel();
        }
        mUIFacade.PlayButtonAudioClip();
        isInBigLevelPanel = true;
    }

    public void ToHelpPanel()
    {
        mUIFacade.PlayButtonAudioClip();
        mUIFacade.currentScenePanelDict[StringManager.HelpPanel].EnterPanel();
    }
}
