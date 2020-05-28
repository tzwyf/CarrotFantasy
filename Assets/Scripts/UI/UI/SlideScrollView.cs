using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// 单滑
/// </summary>
public class SlideScrollView : MonoBehaviour,IBeginDragHandler,IEndDragHandler {

    private RectTransform contentTrans;
    private float beginMousePositionX;
    private float endMousePositionX;
    private ScrollRect scrollRect;

    public int cellLength;
    public int spacing;
    public int leftOffset;
    private float moveOneItemLength;

    private Vector3 currentContentLocalPos;//上一次的位置
    private Vector3 contentInitPos;//Content初始位置
    private Vector2 contentTransSize;//Content初始大小

    public int totalItemNum;
    private int currentIndex;

    public Text pageText;

    public bool needSendMessage;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        contentTrans = scrollRect.content;
        moveOneItemLength = cellLength + spacing;
        currentContentLocalPos = contentTrans.localPosition;
        contentTransSize = contentTrans.sizeDelta;
        contentInitPos = contentTrans.localPosition;
        currentIndex = 1;
        if (pageText != null)
        {
            pageText.text = currentIndex.ToString() + "/" + totalItemNum;
        }
    }

    public void Init()
    {
        currentIndex = 1;
       
        if (contentTrans!=null)
        {
            contentTrans.localPosition = contentInitPos;
            currentContentLocalPos = contentInitPos;
            if (pageText != null)
            {
                pageText.text = currentIndex.ToString() + "/" + totalItemNum;
            }
        }
    }

    /// <summary>
    /// 通过拖拽与松开来达成翻页效果
    /// </summary>
    /// <param name="eventData"></param>

    public void OnEndDrag(PointerEventData eventData)
    {
        endMousePositionX = Input.mousePosition.x;
        float offSetX = 0;
        float moveDistance = 0;//当次需要滑动的距离
        offSetX = beginMousePositionX - endMousePositionX;

        if (offSetX>0)//右滑
        {
            if (currentIndex>=totalItemNum)
            {
                return;
            }
            if (needSendMessage)
            {
                UpdatePanel(true);
            }
            
            moveDistance = -moveOneItemLength;
            currentIndex++;
        }
        else//左滑
        {
            if (currentIndex<=1)
            {
                return;
            }
            if (needSendMessage)
            {
                UpdatePanel(false);
            }
            moveDistance = moveOneItemLength;
            currentIndex--;
        }
        if (pageText != null)
        {
            pageText.text = currentIndex.ToString() + "/" + totalItemNum;
        }
        DOTween.To(()=>contentTrans.localPosition,lerpValue=>contentTrans.localPosition=lerpValue,currentContentLocalPos+new Vector3(moveDistance,0,0),0.5f).SetEase(Ease.OutQuint);
        currentContentLocalPos += new Vector3(moveDistance, 0, 0);
        //只能存在于此项目
        GameManager.Instance.audioSourceManager.PlayPagingAudioClip();
    }

    /// <summary>
    /// 按钮来控制翻书效果
    /// </summary>

    public void ToNextPage()
    {
        float moveDistance = 0;
        if (currentIndex>=totalItemNum)
        {
            return;
        }

        moveDistance = -moveOneItemLength;
        currentIndex++;
        if (pageText!=null)
        {
            pageText.text = currentIndex.ToString() + "/" + totalItemNum;
        }
        if (needSendMessage)
        {
            UpdatePanel(true);
        }
        DOTween.To(() => contentTrans.localPosition, lerpValue => contentTrans.localPosition = lerpValue, currentContentLocalPos + new Vector3(moveDistance, 0, 0), 0.5f).SetEase(Ease.OutQuint);
        currentContentLocalPos += new Vector3(moveDistance, 0, 0);
    }

    public void ToLastPage()
    {
        float moveDistance = 0;
        if (currentIndex <=1)
        {
            return;
        }

        moveDistance = moveOneItemLength;
        currentIndex--;
        if (pageText != null)
        {
            pageText.text = currentIndex.ToString() + "/" + totalItemNum;
        }
        if (needSendMessage)
        {
            UpdatePanel(false);
        }
        DOTween.To(() => contentTrans.localPosition, lerpValue => contentTrans.localPosition = lerpValue, currentContentLocalPos + new Vector3(moveDistance, 0, 0), 0.5f).SetEase(Ease.OutQuint);
        currentContentLocalPos += new Vector3(moveDistance, 0, 0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        beginMousePositionX = Input.mousePosition.x;
    }

    //设置Content的大小
    public void SetContentLength(int itemNum)
    {
        contentTrans.sizeDelta = new Vector2(contentTrans.sizeDelta.x+(cellLength+spacing)*(itemNum-1),contentTrans.sizeDelta.y);
        totalItemNum = itemNum;
    }

    //初始化Content的大小
    public void InitScrollLength()
    {
        contentTrans.sizeDelta = contentTransSize;
    }

    //发送翻页信息的方法
    public void UpdatePanel(bool toNext)
    {
        if (toNext)
        {
            gameObject.SendMessageUpwards("ToNextLevel");
        }
        else
        {
            gameObject.SendMessageUpwards("ToLastLevel");
        }
    }
}
