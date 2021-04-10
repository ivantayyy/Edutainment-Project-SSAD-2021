using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentNameElement : MonoBehaviour
{
    // Start is called before the first frame update
    public Text studentName;
    private string userid;
    public void NewStudentNameElement(string _studentName,string _userid)
    {
        this.studentName.text = _studentName;
        this.userid = _userid;
    }

    public void OnClick()
    {
        UnityEngine.Debug.Log(studentName.text);
        UnityEngine.Debug.Log($"Onclick function: {userid}");
        //loads specific student info
        StudentSummaryReportManager.instance.loadStudentInfo(userid);
        MainMenu.instance.studentSummaryReportScreen();

    }
}
