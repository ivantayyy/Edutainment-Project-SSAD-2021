using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummaryReportManager : MonoBehaviour
{
    [Header("SummaryReport")]
    public GameObject classElement;
    public GameObject studentNameElement;
    public GameObject studentDataElement;
    public Transform classContent;
    public Transform studentNameContent;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    //Loads studentNames for the summary report
    //public void LoadStudentNameBtn(string className)
    //{
    //    StartCoroutine(FirebaseManager.LoadStudentNames(className));
    //}

    //loads class list


    //loads all student names in class

    //public static IEnumerator LoadStudentNames(string className)
    //{
    //    UnityEngine.Debug.Log("reached inside LOAdstudentNames function");
    //    var task = DBreference.Child("Student").OrderByChild("classSubscribed").EqualTo(className).GetValueAsync();
    //    yield return new WaitUntil(predicate: () => task.IsCompleted);

    //    if (task.Exception != null)
    //    {
    //        UnityEngine.Debug.Log(task.Exception);
    //    }
    //    else
    //    {

    //        DataSnapshot snapshot = task.Result;
    //        UnityEngine.Debug.Log(task.Result.GetRawJsonValue());
    //        foreach (Transform child in studentNameContent.transform)
    //        {
    //            Destroy(child.gameObject);
    //            UnityEngine.Debug.Log("Destroyed a child");
    //        }
    //        //Need to instantiate prefab dynamically
    //        foreach (DataSnapshot student in snapshot.Children.Reverse<DataSnapshot>())
    //        {
    //            string username = student.Child("username").Value.ToString();
    //            UnityEngine.Debug.Log(username);
    //            GameObject nameBoardElement = Instantiate(studentNameElement, this.studentNameContent);
    //            nameBoardElement.GetComponent<StudentNameElement>().NewStudentNameElement(username);
    //        }
    //    }
    //}
    public void summaryReportButton()
    {
        LoadClassList();
        UIManager.instance.summaryReportScreen();
    }
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
