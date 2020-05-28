using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOP : MonoBehaviour {

	// Use this for initialization
	void Start () {
        BaseHero myHero = new Leblanc();
        myHero.SkillE();
        myHero.hp = 10;
	}
	
}

public interface IHero
{
    void SkillQ();
    void SkillW();
    void SkillE();
    void SkillR();
}

public class BaseHero : IHero
{
    public int hp;

    public virtual void SkillE()
    {
        Debug.Log("玩家按下E键");
    }

    public void SkillQ()
    {
        Debug.Log("玩家按下Q键");
    }

    public void SkillR()
    {
        Debug.Log("玩家按下R键");
    }

    public void SkillW()
    {
        Debug.Log("玩家按下W键");
    }
}

public class Leblanc : BaseHero
{

    public override void SkillE()
    {
        base.SkillE();
        Debug.Log("幻影锁链");
    }

    //public void SkillQ()
    //{
    //    Debug.Log("恶意魔印");
    //}

    //public void SkillR()
    //{
    //    Debug.Log("故技重施");
    //}

    //public void SkillW()
    //{
    //    Debug.Log("魔影迷踪");
    //}
}

public class Zed : BaseHero
{
    //public void SkillE()
    //{
    //    Debug.Log("禁奥义 鬼斩");
    //}

    //public void SkillQ()
    //{
    //    Debug.Log("禁奥义 诸刃");
    //}

    //public void SkillR()
    //{
    //    Debug.Log("禁奥义 瞬狱影杀阵");
    //}

    //public void SkillW()
    //{
    //    Debug.Log("禁奥义 分身");
    //}
}
