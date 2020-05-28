using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 菜单页面
/// </summary>
public class MenuPage : MonoBehaviour {

    private NormalModelPanel normalMordelpanel;

    private void Awake()
    {
        normalMordelpanel = transform.GetComponentInParent<NormalModelPanel>();

    }

    public void GoOn()
    {
        GameManager.Instance.audioSourceManager.PlayButtonAudioClip();
        GameController.Instance.isPause = false;
        transform.gameObject.SetActive(false);
    }

    public void Replay()
    {
        normalMordelpanel.Replay();
    }

    public void ChooseOtherLevel()
    {
        normalMordelpanel.ChooseOtherLevel();
    }
}
