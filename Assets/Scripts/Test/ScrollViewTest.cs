using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollViewTest : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler {

    private ScrollRect scrollRect;
    private RectTransform contentRectTrans;

	// Use this for initialization
	void Start () {
        scrollRect = GetComponent<ScrollRect>();

        //关于RectTransform的探究
        contentRectTrans = scrollRect.content;

        //当前UI的世界坐标
        Debug.Log("当前UI的世界坐标:" + contentRectTrans.position);
        //当前UI的局部坐标
        Debug.Log("当前UI的局部坐标:" + contentRectTrans.localPosition);
        //当前UI的宽度（从左到右的长度）
        //Debug.Log("当前UI的宽度:"+contentRectTrans.rect.right);
        Debug.Log("当前UI的宽度:" + contentRectTrans.rect.xMax);
        Debug.Log("当前UI的宽度:" + contentRectTrans.rect.width);

        //当前UI的左坐标
        Debug.Log("当前UI的左坐标:" + contentRectTrans.rect.xMin);
        Debug.Log("当前UI的左坐标:" + contentRectTrans.rect.x);//并不是当前UI的x的局部坐标

        //当前UI的高度
        Debug.Log("当前UI的高度:" + contentRectTrans.rect.height);

        //这里要注意，他只是当前transform的x轴向的方向
        //就像是tranform.right自身方向的右方
        Debug.Log(contentRectTrans.right);

        //当前UI底部相对于顶部的相对长度，负数为向下延展，同理则反
        Debug.Log(contentRectTrans.rect.y);

        //当前UI的宽高
        Debug.Log(contentRectTrans.sizeDelta);
        Debug.Log(contentRectTrans.sizeDelta.x);
        Debug.Log(contentRectTrans.sizeDelta.y);

        //宽度应该是我们想要增加的值
        contentRectTrans.sizeDelta = new Vector2(320,300);

        //水平滚动位置为0到1的值，0表示左侧
        scrollRect.horizontalNormalizedPosition =1;
        scrollRect.onValueChanged.AddListener(PrintValue);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void PrintValue(Vector2 vector2)
    {
        Debug.Log("传递进来的参数值"+vector2);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("结束滑动");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("滑动中");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("开始滑动");
    }
}
