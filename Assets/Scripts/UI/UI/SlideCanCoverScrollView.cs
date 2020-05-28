using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SlideCanCoverScrollView : MonoBehaviour,IBeginDragHandler,IEndDragHandler {

    private float contentLength;//容器长度
    private float beginMousePostionX;
    private float endMousePositionX;
    private ScrollRect scrollRect;
    private float lastProportion;//上一个位置比例

    public int cellLength;//每个单元格长度
    public int spacing;//间隙
    public int leftOffset;//左偏移量
    private float upperLimit;//上限值
    private float lowerLimit;//下限值
    private float firstItemLength;//移动第一个单元格的距离
    private float oneItemLength;//滑动一个单元格需要的距离
    private float oneItemProportion;//滑动一个单元格所占比例

    public int totalItemNum;//共有几个单元格
    private int currentIndex;//当前单元格索引

    public Text pageText;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        contentLength = scrollRect.content.rect.xMax - 2 * leftOffset - cellLength;
        firstItemLength = cellLength / 2 + leftOffset;
        oneItemLength = cellLength + spacing;
        oneItemProportion = oneItemLength / contentLength;
        upperLimit=1- firstItemLength / contentLength;
        lowerLimit = firstItemLength / contentLength;
        currentIndex = 1;
        scrollRect.horizontalNormalizedPosition = 0;
        if (pageText != null)
        {
            pageText.text = currentIndex.ToString() + "/" + totalItemNum;
        }
    }

    public void Init()
    {           
        lastProportion = 0;
        currentIndex = 1;
        if (scrollRect != null)
        {
            scrollRect.horizontalNormalizedPosition = 0;
            pageText.text = currentIndex.ToString() + "/" + totalItemNum;
        }
    } 

    public void OnEndDrag(PointerEventData eventData)
    {
        float offSetX = 0;
        endMousePositionX = Input.mousePosition.x;
        offSetX = (beginMousePostionX - endMousePositionX)*2;
        //Debug.Log("offSetX:" + offSetX);
        //Debug.Log("firstItemLength:" + firstItemLength);
        if (Mathf.Abs(offSetX)>firstItemLength)//执行滑动动作的前提是要大于第一个需要滑动的距离
        {
            if (offSetX>0)//右滑
            {
                if (currentIndex>=totalItemNum)
                {
                    return;
                }
                int moveCount = 
                    (int)((offSetX - firstItemLength) / oneItemLength) + 1;//当次可以移动的格子数目
                currentIndex += moveCount;
                if (currentIndex>=totalItemNum)
                {
                    currentIndex = totalItemNum;
                }
                //当次需要移动的比例:上一次已经存在的单元格位置
                //的比例加上这一次需要去移动的比例
                lastProportion += oneItemProportion * moveCount;
                if (lastProportion>=upperLimit)
                {
                    lastProportion = 1;
                }
            }
            else //左滑
            {
                if (currentIndex <=1)
                {
                    return;
                }
                int moveCount =
                    (int)((offSetX + firstItemLength) / oneItemLength) - 1;//当次可以移动的格子数目
                currentIndex += moveCount;
                if (currentIndex <=1)
                {
                    currentIndex = 1;
                }
                //当次需要移动的比例:上一次已经存在的单元格位置
                //的比例加上这一次需要去移动的比例
                lastProportion += oneItemProportion * moveCount;
                if (lastProportion <= lowerLimit)
                {
                    lastProportion = 0;
                }
            }
            if (pageText!=null)
            {
                pageText.text = currentIndex.ToString() + "/" + totalItemNum;
            }
            
        }

        DOTween.To(() => scrollRect.horizontalNormalizedPosition, lerpValue => scrollRect.horizontalNormalizedPosition = lerpValue, lastProportion, 0.5f).SetEase(Ease.OutQuint);
        GameManager.Instance.audioSourceManager.PlayPagingAudioClip();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        beginMousePostionX = Input.mousePosition.x;
    }
}
