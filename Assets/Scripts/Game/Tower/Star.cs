using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 星星塔
/// </summary>
public class Star : TowerPersonalProperty {

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (GameController.Instance.isPause || GameController.Instance.gameOver)
        {
            return;
        }
        if (targetTrans == null)
        {
            return;
        }
        if (timeVal >= attackCD / GameController.Instance.gameSpeed)
        {
            timeVal = 0;
            Attack();
        }
        else
        {
            timeVal += Time.deltaTime;
        }
    }
}
