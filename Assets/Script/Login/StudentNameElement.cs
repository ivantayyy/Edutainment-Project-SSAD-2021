using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentNameElement : MonoBehaviour
{
    // Start is called before the first frame update
    public Text studentName;

    public void NewStudentNameElement(string _studentName)
    {
        this.studentName.text = _studentName;
    }

    public void OnClick()
    {
        UnityEngine.Debug.Log(studentName.text);
    }
}
