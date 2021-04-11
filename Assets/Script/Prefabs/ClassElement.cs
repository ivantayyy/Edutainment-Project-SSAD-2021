using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassElement : MonoBehaviour
{
    // Start is called before the first frame update
    public Text className;

    public void NewClassElement(string classname)
    {
        this.className.text = classname;
    }
    async public void LoadStudentNameBtn()
    {
        string classname = className.text;
        UnityEngine.Debug.Log(classname);
        var Task = SummaryReportManager.instance.LoadStudentNamesAsync(classname);
        await Task;
    }


}