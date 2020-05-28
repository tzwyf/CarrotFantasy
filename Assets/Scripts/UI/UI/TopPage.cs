using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 顶部UI显示页面
/// </summary>
public class TopPage : MonoBehaviour {

    //引用
    private Text tex_coin;
    private Text tex_roundCount;
    private Text tex_TotalCount;
    private Image img_Btn_GameSpeed;
    private Image img_Btn_Pause;
    private GameObject emp_pauseGo;
    private GameObject emp_playingTextGo;
    private NormalModelPanel normalMordelpanel;

    //按钮图片切换资源
    public Sprite[] btn_gameSpeedSprites;
    public Sprite[] btn_pauseSprites;

    //开关
    private bool isNormalSpeed;
    private bool isPause;

    private void Awake()
    {
        normalMordelpanel = transform.GetComponentInParent<NormalModelPanel>();
        tex_coin = transform.Find("Tex_Coin").GetComponent<Text>();
        tex_roundCount = transform.Find("Emp_PlayingText").Find("Tex_RoundCount").GetComponent<Text>();
        tex_TotalCount = transform.Find("Emp_PlayingText").Find("Tex_TotalCount").GetComponent<Text>();
        img_Btn_GameSpeed = transform.Find("Btn_GameSpeed").GetComponent<Image>();
        img_Btn_Pause = transform.Find("Btn_Pause").GetComponent<Image>();
        emp_pauseGo = transform.Find("Emp_Pause").gameObject;
        emp_playingTextGo = transform.Find("Emp_PlayingText").gameObject;

    }

    private void OnEnable()
    {
        UpdateCoinText();
        tex_TotalCount.text = normalMordelpanel.totalRound.ToString();
        img_Btn_Pause.sprite = btn_pauseSprites[0];
        img_Btn_GameSpeed.sprite = btn_gameSpeedSprites[0];
        isPause = false;
        isNormalSpeed = true;
        emp_pauseGo.SetActive(false);
        emp_playingTextGo.SetActive(true);
    }
    //更新UI文本
    public void UpdateCoinText()
    {
        tex_coin.text = GameController.Instance.coin.ToString();
    }

    public void UpdateRoundText()
    {
        normalMordelpanel.ShowRoundText(tex_roundCount);
    }

    //改变游戏速度
    public void ChangeGameSpeed()
    {
        GameManager.Instance.audioSourceManager.PlayButtonAudioClip();
        isNormalSpeed = !isNormalSpeed;
        if (isNormalSpeed)
        {
            GameController.Instance.gameSpeed = 1;
            img_Btn_GameSpeed.sprite = btn_gameSpeedSprites[0];
        }
        else
        {
            GameController.Instance.gameSpeed = 2;
            img_Btn_GameSpeed.sprite = btn_gameSpeedSprites[1];
        }
    }

    //游戏暂停
    public void PauseGame()
    {
        GameManager.Instance.audioSourceManager.PlayButtonAudioClip();
        isPause = !isPause;
        if (isPause)
        {
            GameController.Instance.isPause = true;
            img_Btn_Pause.sprite = btn_pauseSprites[1];
            emp_pauseGo.SetActive(true);
            emp_playingTextGo.SetActive(false);
        }
        else
        {
            GameController.Instance.isPause = false;
            img_Btn_Pause.sprite = btn_pauseSprites[0];
            emp_pauseGo.SetActive(false);
            emp_playingTextGo.SetActive(true);
        }
    }

    public void ShowMenu()
    {
        normalMordelpanel.ShowMenuPage();
    }
}
