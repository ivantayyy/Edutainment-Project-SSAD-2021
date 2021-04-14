using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class AssignmentResultsManager : MonoBehaviour
{
    public GameObject classElement;
    //public GameObject studentNameElement;
    public Transform studentNameContent;
    public Transform AssignmentBoardContent;
    public Text attemptsText;
    public Text maxPtText;
    public Text ptAttemptText;
    private string assignmentID;
    private string studentName;

    public static AssignmentResultsManager instance;
    // Start is called before the first frame update
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            LoadAssignmentList();
        }
    }
    public async void LoadAssignmentList()
    {
        List<string> AssignmentList = await FirebaseManager.getAllAssignmentName();
        ClassElement element = new ClassElement();
        foreach (Transform child in this.AssignmentBoardContent.transform)
        {
            UnityEngine.Debug.Log("reached before transform loop");
            Destroy(child.gameObject);
            UnityEngine.Debug.Log("reached after transform loop");
        }
        foreach (string assignmentName in AssignmentList)
        {
            this.assignmentID = assignmentName;
            //get instantiated gameobject
            GameObject classBoardElement = Instantiate(classElement, this.AssignmentBoardContent);
            //add and get script to gameobject
            element = classBoardElement.AddComponent<ClassElement>();
            //assign script's text to gameobject's child's text
            element.TextName = classBoardElement.transform.GetChild(0).GetComponent<Text>();
            //edit text
            element.NewElement(assignmentName);
            //add onclick function to gameobject which will load student names
            classBoardElement.GetComponent<Button>().onClick.AddListener(async delegate
            {
                await LoadStudentNamesAsync(assignmentName);

            });
        }
    }
    //Button that Loads studentNames for the summary report

    //loads all student names in class

    async public Task LoadStudentNamesAsync(string className)
    {
        Debug.Log("loading studentName async");
        List<string> StudentNames;
        ClassElement element = new ClassElement();
        var StudentNamesTask = FirebaseManager.getAllStudentFromAssignments(className);
        StudentNames = await StudentNamesTask;

        foreach (Transform child in studentNameContent.transform)
        {
            Destroy(child.gameObject);
            UnityEngine.Debug.Log("Destroyed a child");
        }
        //Need to instantiate prefab dynamically
        foreach (var studentName in StudentNames)
        {
            this.studentName = studentName;
            Debug.Log("for sNames in sNames "+(string)studentName);
            GameObject classBoardElement = Instantiate(classElement, this.studentNameContent);
            //add and get script to gameobject
            element = classBoardElement.AddComponent<ClassElement>();
            //assign script's text to gameobject's child's text
            element.TextName = classBoardElement.transform.GetChild(0).GetComponent<Text>();
            //edit text
            element.NewElement(studentName);
            //add onclick function to gameobject which will load student names
            classBoardElement.GetComponent<Button>().onClick.AddListener(async delegate
            {
                //await LoadStudentNamesAsync(studentName);
                getAssignmentResults(assignmentID, studentName);

            });
        }
    }

    public async void getAssignmentResults(string assignmentID, string sName)
    {
        AssignmentResults results = await FirebaseManager.getAssignmentResults(assignmentID,sName);
        attemptsText.text = "Attempts: " + results.attempts;
        maxPtText.text = "Max Score: " + results.maxPoint;
        string ptAttemptStr = "";
        int i = 1;
        foreach(int pts in results.points)
        {
            ptAttemptStr += "Attempt " + i + ": " + pts + "\n";
            i++;
        }
        ptAttemptText.text = ptAttemptStr;
    }

    public void backBtn()
    {
        SceneManager.LoadScene("Teacher Menu");
    }

}
