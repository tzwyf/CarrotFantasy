using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 负责控制音乐的播放和停止以及游戏中各种音效的播放
/// </summary>
public class AudioSourceManager
{
    private AudioSource[] audioSource;//0.播放BGMusic 1.播放特效音
    private bool playEffectMusic = true;
    private bool playBGMusic = true;

    //构造函数
    public AudioSourceManager()
    {
        audioSource = GameManager.Instance.GetComponents<AudioSource>();
    }

    //播放背景音乐
    public void PlayBGMusic(AudioClip audioClip)
    {
        if (!audioSource[0].isPlaying || audioSource[0].clip != audioClip)
        {
            audioSource[0].clip = audioClip;
            audioSource[0].Play();
        }
    }

    //播放音效
    public void PlayEffectMusic(AudioClip audioClip)
    {
        if (playEffectMusic)
        {
            audioSource[1].PlayOneShot(audioClip);
        }
    }

    public void CloseBGMusic()
    {
        audioSource[0].Stop();
    }

    public void OpenBGMusic()
    {
        audioSource[0].Play();
    }

    public void CloseOrOpenBGMusic()
    {
        playBGMusic = !playBGMusic;
        if (playBGMusic)
        {
            OpenBGMusic();
        }
        else
        {
            CloseBGMusic();
        }
    }

    public void CloseOrOpenEffectMusic()
    {
        playEffectMusic = !playEffectMusic;
    }
    //按钮音效播放
    public void PlayButtonAudioClip()
    {
        PlayEffectMusic(GameManager.Instance.factoryManager.audioClipFactory.GetSingleResources("Main/Button"));
    }
    //翻书音效播放
    public void PlayPagingAudioClip()
    {
        PlayEffectMusic(GameManager.Instance.factoryManager.audioClipFactory.GetSingleResources("Main/Paging"));
    }

}
