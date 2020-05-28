using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 萝卜
/// </summary>
public class Carrot : MonoBehaviour {

    //萝卜的不同状态
    private Sprite[] sprites;
    private Animator animator;
    private float timeVal;
    private SpriteRenderer sr;
    private Text hpText;

	// Use this for initialization
	void Start () {
        sprites = new Sprite[7];
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i] = GameController.Instance.GetSprite("NormalMordel/Game/Carrot/"+i.ToString());
        }
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        hpText = transform.Find("HpCanvas").Find("Text").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (GameController.Instance.carrotHp<10)
        {
            animator.enabled = false;
        }

        if (timeVal >= 3)
        {
            animator.Play("Idle");
            timeVal = 0;
        }
        else
        {
            timeVal += Time.deltaTime;
        }
    }

    private void OnMouseDown()
    {
        if (GameController.Instance.carrotHp>=10)
        {
            animator.Play("Touch");
            int randomNum = Random.Range(1,4);
            GameController.Instance.PlayEffectMusic("NormalMordel/Carrot/"+randomNum.ToString());
        }
    }

    public void UpdateCarrotUI()
    {
        int hp = GameController.Instance.carrotHp;
        hpText.text = hp.ToString();
        if (hp>=7&&hp<10)
        {
            sr.sprite = sprites[6];
        }
        else if(hp<7&&hp>0)
        {
            sr.sprite = sprites[hp - 1];
        }
        else
        {
            //游戏结束
            GameController.Instance.normalModelpanel.ShowGameOverPage();
            GameController.Instance.gameOver = true;
        }
    }
}
