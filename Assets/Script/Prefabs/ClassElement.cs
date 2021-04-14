using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassElement : MonoBehaviour
{
    // Start is called before the first frame update
    public Text TextName;

    public void NewElement(string classname)
    {
        this.TextName.text = classname;
    }
    /*async public void OnClick()
    {
        Debug.Log("clicked");
        string classname = className.text;
        UnityEngine.Debug.Log(classname);
        var Task = SummaryReportManager.instance.LoadStudentNamesAsync(classname);
        await Task;
    }*/


}