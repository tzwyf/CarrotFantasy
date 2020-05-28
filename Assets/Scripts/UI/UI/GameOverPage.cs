using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏失败结束页面
/// </summary>
public class GameOverPage : MonoBehaviour {

    private Text tex_RoundCount;
    private Text tex_TotalCount;
    private Text tex_CurrentLevel;
    private NormalModelPanel normalModelPanel;

    private void Awake()
    {
        normalModelPanel = transform.GetComponentInParent<NormalModelPanel>();
        tex_RoundCount = transform.Find("Tex_RoundCount").GetComponent<Text>();
        tex_TotalCount = transform.Find("Tex_TotalCount").GetComponent<Text>();
        tex_CurrentLevel = transform.Find("Tex_CurrentLevel").GetComponent<Text>();
    }

    private void OnEnable()
    {
        tex_TotalCount.text = normalModelPanel.totalRound.ToString();
        tex_CurrentLevel.text = (GameController.Instance.currentStage.mLevelID+(GameController.Instance.currentStage.mBigLevelID-1)*5).ToString();
        normalModelPanel.ShowRoundText(tex_RoundCount);
    }

    public void Replay()
    {
        normalModelPanel.Replay();
    }

    public void ChooseOtherLevel()
    {
        normalModelPanel.ChooseOtherLevel();
    }
}
