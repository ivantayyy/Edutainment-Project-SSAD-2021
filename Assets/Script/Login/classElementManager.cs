using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class classElementManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Text className;
    public void LoadStudentNameBtn()
    {
        string classname = className.text;
        UnityEngine.Debug.Log(classname);
        SummaryReportManager.instance.LoadStudentNames(classname);
    }


}
