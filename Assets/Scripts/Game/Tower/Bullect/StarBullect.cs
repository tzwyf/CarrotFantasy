using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 星星子弹
/// </summary>
public class StarBullect : MonoBehaviour {

    public int attackValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.activeSelf)
        {
            return;
        }
        if (collision.tag=="Monster"||collision.tag=="Item")
        {
            collision.SendMessage("TakeDamage", attackValue);
        }
    }
}
