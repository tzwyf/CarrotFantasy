using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class JsonTest : MonoBehaviour {

    private AppOfTriggerPhone appOfTriggerPhone;

    // Use this for initialization
    void Start() {
        //简单Json的存储与读取

        ////appOfTriggerPhone = new AppOfTriggerPhone
        ////{
        ////    appNum = 3,
        ////    phoneState = true,
        ////    appList = new List<string>()
        ////    {
        ////        "抖音","BiliBili","绝地求生"
        ////    }
        ////};
        //appOfTriggerPhone = new AppOfTriggerPhone();
        ////SaveByJson();
        //appOfTriggerPhone = LoadByJson();
        //Debug.Log(appOfTriggerPhone.appNum);
        //Debug.Log(appOfTriggerPhone.phoneState);
        //foreach (var item in appOfTriggerPhone.appList)
        //{
        //    Debug.Log(item);
        //}

        //复杂Json的信息读取
        //appOfTriggerPhone = new AppOfTriggerPhone
        //{
        //    appNum = 3,
        //    phoneState=true,
        //    appList=new List<AppProperty>()
        //};
        //AppProperty appProperty = new AppProperty
        //{
        //    appName = "抖音",
        //    triggerID = "Trigger",
        //    triggerFavour = true,
        //    useTimeList = new List<int> { 6, 7, 8 }
        //};
        //appOfTriggerPhone.appList.Add(appProperty);
        //SaveByJson();
        appOfTriggerPhone = LoadByJson();
        Debug.Log(appOfTriggerPhone.appNum);
        Debug.Log(appOfTriggerPhone.phoneState);
        foreach (var item in appOfTriggerPhone.appList)
        {
            Debug.Log(item);
            Debug.Log(item.appName);
            Debug.Log(item.triggerFavour);
            Debug.Log(item.triggerID);
            foreach (var itemGo in item.useTimeList)
            {
                Debug.Log(itemGo);
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }

    //存贮Json信息文件
    private void SaveByJson()
    {
        string filePath = Application.dataPath + "/Resources/Json" + "/AppOfTrigger.json";
        //利用JsonMapper将信息类对象转化成json格式的字符串
        string saveJsonStr = JsonMapper.ToJson(appOfTriggerPhone);
        //创建一个文件流去将字符串写入一个文件中
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveJsonStr);
        sw.Close();
    }

    //读取Json的信息文件
    private AppOfTriggerPhone LoadByJson()
    {
        AppOfTriggerPhone appGo = new AppOfTriggerPhone();
        string filePath=Application.dataPath + "/Resources/Json" + "/AppOfTrigger.json";
        if (File.Exists(filePath))
        {
            StreamReader sr = new StreamReader(filePath);

            string jsonStr = sr.ReadToEnd();

            sr.Close();
            appGo = JsonMapper.ToObject<AppOfTriggerPhone>(jsonStr);
        }

        if (appGo==null)
        {
            Debug.Log("读取Json信息失败");
        }
        return appGo;
    }
}
