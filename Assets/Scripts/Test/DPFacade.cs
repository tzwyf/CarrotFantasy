using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPFacade : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Principal principal = new Principal();
        principal.OrderTeacherToDoTask(); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

//上次管理
public class Principal
{
    private Teacher teacher = new Teacher();
    public void OrderTeacherToDoTask()
    {
        teacher.OrderStudentsToSummary();
    }
}

//外观角色
public class Teacher
{
    private Monitor monitor = new Monitor();
    private LeagueSecretary leagueSecretary = new LeagueSecretary();
    public void OrderStudentsToSummary()
    {
        monitor.WriteSummary();
        leagueSecretary.WriteSummary();
    }
}

public class Monitor
{
    public void WriteSummary()
    {
        Debug.Log("班长的总结");
    }
}

public class LeagueSecretary
{
    public void WriteSummary()
    {
        Debug.Log("团支书的总结");
    }
}
