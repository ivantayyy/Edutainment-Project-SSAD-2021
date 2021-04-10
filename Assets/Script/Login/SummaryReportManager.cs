using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryReportManager : MonoBehaviour
{
    [Header("SummaryReport")]
    public GameObject classElement;
    public GameObject studentNameElement;
    public Transform classContent;
    public Transform studentNameContent;

    public static SummaryReportManager instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            LoadClassList();
            instance = this;
        }
    }
    
    //loads class list
    private void LoadClassList()
    {
        List<string> classList = new List<string>() {
            "FS6","FS7","FS8","FS9"
        };
        foreach (Transform child in this.classContent.transform)
        {
            UnityEngine.Debug.Log("reached before transform loop");
            Destroy(child.gameObject);
            UnityEngine.Debug.Log("reached after transform loop");
        }
        foreach (string className in classList)
        {
            GameObject classBoardElement = Instantiate(classElement, this.classContent);
            classBoardElement.GetComponent<ClassElement>().NewClassElement(className);
        }
    }
    //Button that Loads studentNames for the summary report
    

    

    //loads all student names in class

    public async void LoadStudentNames(string className)
    {
        UnityEngine.Debug.Log("reached inside LOAdstudentNames function");
        Dictionary<string,string> StudentNames;
        StudentNames = await FirebaseManager.loadStudentNames(className);
        foreach (Transform child in studentNameContent.transform)
        {
            Destroy(child.gameObject);
            UnityEngine.Debug.Log("Destroyed a child");
        }
        //Need to instantiate prefab dynamically
        foreach (var item in StudentNames)
        {
            string username = item.Value;
            string userid = item.Key;
            UnityEngine.Debug.Log($"LoadStudentNames() username:{username}");
            UnityEngine.Debug.Log($"LoadStudentNames() userid:{userid}");
            GameObject nameBoardElement = Instantiate(studentNameElement, studentNameContent);
            nameBoardElement.GetComponent<StudentNameElement>().NewStudentNameElement(username,userid);
        }
    }

    public void summaryReportButton()
    {
        LoadClassList();
        UIManager.instance.summaryReportScreen();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
