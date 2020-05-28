using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPFactory : MonoBehaviour {

	// Use this for initialization
	void Start () {
        FactoryIPhone8 factoryIPhone8 = new FactoryIPhone8();
        factoryIPhone8.CreateIPhone();
        factoryIPhone8.CreateIPhoneCharger();
        FactoryIPhoneX factoryIPhoneX = new FactoryIPhoneX();
        factoryIPhoneX.CreateIPhone();
        factoryIPhoneX.CreateIPhoneCharger();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

//工厂模式分为简单工厂模式，工厂方法模式跟抽象工厂模式

//抽象工厂模式
//手机
public interface IPhone
{
    
}

public class IPhone8 : IPhone
{
    public IPhone8()
    {
        Debug.Log("IPhone8");
    }

}

public class IPhoneX : IPhone
{
    public IPhoneX()
    {
        Debug.Log("IPhoneX");
    }

}

//充电器
public interface IPhoneCharger
{

}

public class IPhone8Charger : IPhoneCharger
{
    public IPhone8Charger()
    {
        Debug.Log("IPhone8Charger");
    }

}

public class IPhoneXCharger : IPhoneCharger
{
    public IPhoneXCharger()
    {
        Debug.Log("IPhoneXCharger");
    }

}

public interface IFactory
{
    IPhone CreateIPhone();
    IPhoneCharger CreateIPhoneCharger();
}

public class FactoryIPhone8 : IFactory
{
    public IPhone CreateIPhone()
    {
        return new IPhone8();
    }

    public IPhoneCharger CreateIPhoneCharger()
    {
        return new IPhone8Charger();
    }
}

public class FactoryIPhoneX : IFactory
{
    public IPhone CreateIPhone()
    {
        return new IPhoneX();
    }

    public IPhoneCharger CreateIPhoneCharger()
    {
        return new IPhoneXCharger();
    }
}


//简单工厂模式

//public class IPhone
//{
//    public IPhone()
//    {

//    }
//}

//public class IPhone8 : IPhone
//{
//    public IPhone8()
//    {

//    }
        
//}

//public class IPhoneX : IPhone
//{
//    public IPhoneX()
//    {

//    }

//}

//public interface IFactory
//{
//    IPhone CreateIPhone();
//}

//public class FactoryIPhone8 : IFactory
//{
//    public IPhone CreateIPhone()
//    {
//        return new IPhone8();
//    }
//}

//public class FactoryIPhoneX : IFactory
//{
//    public IPhone CreateIPhone()
//    {
//        return new IPhoneX();
//    }
//}


//使用工厂模式的原因

//public class BullectOne : MonoBehaviour
//{
//    private AudioClip audioClip;
//    private AudioSource audioSource;
//    private void Start()
//    {
//        audioClip = Resources.Load<AudioClip>("*****");
//        audioSource.clip = audioClip;
//        Destroy(gameObject, 4);
//    }
//    //其他特效功能
//}

//public class BullectTwo : MonoBehaviour
//{
//    private AudioClip audioClip;
//    private AudioSource audioSource;
//    private void Start()
//    {
//        audioClip = Resources.Load<AudioClip>("*****");
//        audioSource.clip = audioClip;
//        Destroy(gameObject, 4);
//    }
//    //其他特效功能
//}

//public class ButtonOne : MonoBehaviour
//{
//    private AudioClip audioClip;
//    private AudioSource audioSource;
//    private void Start()
//    {
//        audioClip = Resources.Load<AudioClip>("*****");
//        audioSource.clip = audioClip;
//        Destroy(gameObject, 4);
//    }
//    //其他特效功能
//}
