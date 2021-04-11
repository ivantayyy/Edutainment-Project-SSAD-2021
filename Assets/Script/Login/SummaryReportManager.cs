using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            instance = this;
            Debug.Log("SummaryReportManager instantiated");
            Debug.Log("Loading Class Lists in Summary Report UI");
            LoadClassList();
            Debug.Log("Preloading Class Lists in Summary Report UI Done");

        }
    }
    
    //loads class list
    public void LoadClassList()
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

    async public Task LoadStudentNamesAsync(string className)
    {
        UnityEngine.Debug.Log("reached inside LoadstudentNames function");
        Dictionary<string,string> StudentNames;
        var StudentNamesTask = FirebaseManager.LoadStudentNamesAsync(className);
        StudentNames = await StudentNamesTask;

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
            UnityEngine.Debug.Log($"LoadStudentNamesAsync() in SummaryReport: Successfully loaded  with username:{username} and \n userid:{userid}");
            GameObject nameBoardElement = Instantiate(studentNameElement, studentNameContent);
            nameBoardElement.GetComponent<StudentNameElement>().NewStudentNameElement(username,userid);
            UnityEngine.Debug.Log($"Successfully instantiated classelement on Summary Report with for username: {username}");
        }
    }

    public void summaryReportButton()
    {
        LoadClassList();
        MainMenu.instance.summaryReport();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
