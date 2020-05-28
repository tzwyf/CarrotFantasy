using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseListenTest : MonoBehaviour,IPointerDownHandler {

    //此接口只对UI起作用
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("UI:鼠标按下");
    }

    //OnMouse类型的API只作用于左键
    private void OnMouseDown()
    {
        //Debug.Log("游戏物体：鼠标按下");
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Debug.Log("创建道具");
        //}
        //else if (Input.GetMouseButtonDown(1))
        //{
        //    Debug.Log("创建怪物路点");
        //}
        if (Input.GetKey(KeyCode.P))
        {
            Debug.Log("创建怪物路点");
        }
        else if (Input.GetKey(KeyCode.I))
        {
            Debug.Log("创建道具");
        }
    }

    //private void OnMouseUp()
    //{
    //    if (Input.GetMouseButtonUp(0))
    //    {
    //        Debug.Log("创建怪物路点");
    //    }
    //}


}
