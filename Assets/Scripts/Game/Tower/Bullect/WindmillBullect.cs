using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 风车子弹脚本
/// </summary>
public class WindmillBullect : Bullect {

    private bool hasTarget;
    private float timeVal;

    private void OnEnable()
    {
        hasTarget = false;
        timeVal = 0;
    }

    private void InitTarget()
    {
        if (targetTrans.gameObject.tag=="Item")
        {
            transform.LookAt(targetTrans.position+new Vector3(0,0,3));
        }
        else
        {
            transform.LookAt(targetTrans.position);
        }
        if (transform.eulerAngles.y==0)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,90,transform.eulerAngles.z);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (GameController.Instance.gameOver||timeVal>=2.5f)
        {
            DestoryBullect();
        }
        if (GameController.Instance.isPause)
        {
            return;
        }
        if (timeVal<2.5f)
        {
            timeVal += Time.deltaTime;
        }
        if (hasTarget)
        {
            transform.Translate(transform.forward*moveSpeed*Time.deltaTime,Space.World);
        }
        else
        {
            if (targetTrans!=null&&targetTrans.gameObject.activeSelf)
            {
                hasTarget = true;
                InitTarget();
            }
        }
	}

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Monster" || collision.tag == "Item")
        {
            if (collision.gameObject.activeSelf)
            {
                if (targetTrans.position == null || (collision.tag == "Item" && GameController.Instance.targetTrans == null))
                {
                    return;
                }
                if (collision.tag == "Monster" || (collision.tag == "Item" && GameController.Instance.targetTrans == collision.transform))
                {
                    collision.SendMessage("TakeDamage", attackValue);
                    CreateEffect();
                }
            }
        }
    }
}
