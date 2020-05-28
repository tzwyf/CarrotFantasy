using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternalMethod : MonoBehaviour {

    public enum Teacher
    {
        Siki,
        Ivy,
        Trigger,
        Sandy,
        Druid,
        Lain
    }

    public Teacher teacherKind;

    //经过实验得
    //结论1 调用顺序:先左后右 Inspector>外部赋值(外部调用)>Awake>OnEnable>Start
    //结论2 脚本对象的失活与激活不作用于Awake方法
    //结论3 游戏物体每次从失活到激活状态，Awake与Start只会调用一次，OnEable会在再次激活游戏物体时调用

    private void Awake()
    {
        teacherKind = Teacher.Siki;
        Debug.Log(teacherKind);
    }

    private void OnEnable()
    {
        teacherKind = Teacher.Ivy;
        Debug.Log(teacherKind);
    }

    // Use this for initialization
    void Start()
    {
        teacherKind = Teacher.Trigger;
        Debug.Log(teacherKind);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log(teacherKind);
        }
    }
}
