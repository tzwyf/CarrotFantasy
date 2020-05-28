using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPMediator : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Matchmaker man = new Man(45,1000000,999999,0);
        Matchmaker woman = new Woman(21,0,0,0);
        WomanMatchmakerMediator womanMatchmakerMediator = new WomanMatchmakerMediator(man,woman);

        womanMatchmakerMediator.OfferManInformationToWoman();
        womanMatchmakerMediator.OfferWomanInformationToMan();
        //man.GetInformation(woman);
        //woman.GetInformation(man);
        Debug.Log("男方目前好感度是："+man.m_favor);
        Debug.Log("女方目前好感度是：" + woman.m_favor);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

public class WomanMatchmakerMediator
{
    private Matchmaker m_man;
    private Matchmaker m_woman;

    public WomanMatchmakerMediator(Matchmaker man,Matchmaker woman)
    {
        m_man = man;
        m_woman = woman;
    }

    public void OfferWomanInformationToMan()
    {
        m_man.m_favor += -m_woman.m_age + m_woman.m_familyBG + m_woman.m_money;
    }

    public void OfferManInformationToWoman()
    {
        m_woman.m_favor += -m_man.m_age + m_man.m_familyBG + m_man.m_money;
    }

}

public abstract class Matchmaker
{
    public int m_age;
    public int m_money;
    public int m_familyBG;
    public int m_favor;

    public Matchmaker(int age,int money,int familyBG,int favor)
    {
        m_age = age;
        m_money = money;
        m_familyBG = familyBG;
        m_favor = favor;
    }

    //public abstract void GetInformation(Matchmaker otherMatchmaker);
}

public class Man : Matchmaker
{
    public Man(int age, int money, int familyBG, int favor) : base(age, money, familyBG, favor)
    {
    }

    //public override void GetInformation(Matchmaker otherMatchmaker)
    //{
    //    m_favor += -otherMatchmaker.m_age * 3 + otherMatchmaker.m_money + otherMatchmaker.m_familyBG;
    //}
}

public class Woman : Matchmaker
{
    public Woman(int age, int money, int familyBG, int favor) : base(age, money, familyBG, favor)
    {
    }

    //public override void GetInformation(Matchmaker otherMatchmaker)
    //{
    //    m_favor += -otherMatchmaker.m_age * 3 + otherMatchmaker.m_money + otherMatchmaker.m_familyBG;
    //}
}
