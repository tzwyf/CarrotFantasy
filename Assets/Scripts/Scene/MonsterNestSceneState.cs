using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 怪物窝场景状态
/// </summary>
public class MonsterNestSceneState : BaseSceneState
{
    public MonsterNestSceneState(UIFacade uiFacade) : base(uiFacade)
    {
    }

    public override void EnterScene()
    {
        mUIFacade.AddPanelToDict(StringManager.GameLoadPanel);
        mUIFacade.AddPanelToDict(StringManager.MonsterNestPanel);
        base.EnterScene();
        GameManager.Instance.audioSourceManager.
            PlayBGMusic(GameManager.Instance.factoryManager.audioClipFactory.GetSingleResources("MonsterNest/BGMusic"));
    }

    public override void ExitScene()
    {
        SceneManager.LoadScene(1);
        base.ExitScene();
    }
}
