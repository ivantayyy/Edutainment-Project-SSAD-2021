using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassElement : MonoBehaviour
{
    // Start is called before the first frame update
    public Text class_name;
    public void NewClassElement(string classname)
    {
        this.class_name.text = classname;
    }
    public void OnClick()
    {
        UnityEngine.Debug.Log("you have chosen "+ class_name.text);
        //FirebaseManager.LoadStudentNameBtn(class_name.text);
    }
}
