using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 便便子弹
/// </summary>
public class TshitBullect : Bullect {

    private BullectProperty bullectProperty;

	// Use this for initialization
	void Start () {
        bullectProperty = new BullectProperty
        {
            debuffTime = towerLevel * 0.4f,
            debuffValue = towerLevel * 0.3f
        };
	}

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.activeSelf)
        {
            return;
        }
        if (collision.tag=="Monster")
        {
            collision.SendMessage("DecreaseSpeed",bullectProperty);
        }
        base.OnTriggerEnter2D(collision);
    }
}

public struct BullectProperty
{
    public float debuffTime;
    public float debuffValue;
}
