using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子弹基类
/// </summary>
public class Bullect : MonoBehaviour {

    [HideInInspector]
    public Transform targetTrans;
    public int moveSpeed;
    public int attackValue;
    public int towerID;
    public int towerLevel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        //游戏结束
        if (GameController.Instance.gameOver)
        {
            DestoryBullect();
        }
        //游戏暂停
        if (GameController.Instance.isPause)
        {
            return;
        }
        if (targetTrans==null||!targetTrans.gameObject.activeSelf)
        {
            DestoryBullect();
            return;
        }

        //子弹的移动与转向
        if (targetTrans.gameObject.tag=="Item")
        {
            transform.position = Vector3.Lerp(transform.position,targetTrans.position+new Vector3(0,0,3),
                1/Vector3.Distance(transform.position,targetTrans.position+new Vector3(0,0,3)*Time.deltaTime*moveSpeed*GameController.Instance.gameSpeed));
            transform.LookAt(targetTrans.position+new Vector3(0,0,3));
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetTrans.position,
              1 / Vector3.Distance(transform.position, targetTrans.position * Time.deltaTime * moveSpeed * GameController.Instance.gameSpeed));
            transform.LookAt(targetTrans.position);
        }

        if (transform.eulerAngles.y==0)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,90,transform.eulerAngles.z);
        }
	}

    protected virtual void DestoryBullect()
    {
        targetTrans = null;
        GameController.Instance.PushGameObjectToFactory("Tower/ID"+towerID.ToString()+"/Bullect/"+towerLevel.ToString(),gameObject);
    }

    protected virtual void CreateEffect()
    {
        GameObject effectGo = GameController.Instance.GetGameObjectResource("Tower/ID" + towerID.ToString() + "/Effect/" + towerLevel.ToString());
        effectGo.transform.position = transform.position;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag=="Monster"||collision.tag=="Item")
        {
            if (collision.gameObject.activeSelf)
            {
                if (targetTrans.position==null||(collision.tag=="Item"&&GameController.Instance.targetTrans==null))
                {
                    return;
                }
                if (collision.tag=="Monster"||(collision.tag=="Item"&&GameController.Instance.targetTrans==collision.transform))
                {
                    collision.SendMessage("TakeDamage",attackValue);
                    CreateEffect();
                    DestoryBullect();
                }
            }
        }
    }
}
