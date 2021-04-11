using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentNameElement : MonoBehaviour
{
    // Start is called before the first frame update
    public Text studentName;
    public Text userid;
    public void NewStudentNameElement(string _studentName,string _userid)
    {
        this.studentName.text = _studentName;
        this.userid.text = _userid;
        userid.gameObject.SetActive(false); ;
    }

    public void OnClick()
    {
        UnityEngine.Debug.Log($"Inside StudentNameElement onClick() username: {studentName.text}");
        UnityEngine.Debug.Log($"Inside StudentNameElement onClick() userid: {userid.text}");
        if(userid.text == null)
        {
            Debug.Log("USERIDISNULL");
        }
        //loads specific student info
        StudentSummaryReportManager.instance.loadStudentInfo(userid.text);
        TeacherMenuUIManager.instance.studentSummaryReportScreen();

    }
}
