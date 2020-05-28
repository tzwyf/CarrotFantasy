using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DoTweenTest : MonoBehaviour {

    private Image maskImage;
    private Tween maskTween;

	// Use this for initialization
	void Start () {
        maskImage = GetComponent<Image>();

        ////1.doTween的静态方法
        //DOTween.To(()=>maskImage.color,toColor=>maskImage.color=toColor,new Color(0,0,0,0),2f);
        ////详细分解
        //DOTween.To(
        //    () => 
        //    maskImage.color//我们想改变的对象值
        //    , toColor//每次doTween经过计算得到的结果（当前值到目标值的插值）
        //    => maskImage.color = toColor, //将计算结果赋值给我们想要改变的对象值
        //    new Color(0, 0, 0, 0), 2f);//目标值，完成动画的时间
        
        ////2.doTween直接作用于transform的方法
        //Tween tween= transform.DOLocalMoveX(300,0.5f);
        //tween.PlayForward();
        //tween.PlayBackwards();
        ////结论：直接倒着播放还是先正播再倒播。不存在直接倒播的情况。

        //3.动画的循环使用
        maskTween= transform.DOLocalMoveX(300, 0.5f);
        maskTween.SetAutoKill(false);
        maskTween.Pause();

        //4.动画的事件回调
        Tween tween= transform.DOLocalMoveX(300, 0.5f);
        tween.OnComplete(CompleteMethod);

        //5.设置动画的缓动函数以及循环状跟次数
        tween.SetEase(Ease.InOutBounce);
        tween.SetLoops(-1,LoopType.Incremental);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            //结论：Tween对象的play方法只能播一次(相对于倒播)，不能连续倒播。
            //maskTween.Play();
            maskTween.PlayForward();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            maskTween.PlayBackwards();
        }
	}

    private void CompleteMethod()
    {
        DOTween.To(() => maskImage.color, toColor => maskImage.color = toColor, new Color(0, 0, 0, 0), 2f);
    }
}
