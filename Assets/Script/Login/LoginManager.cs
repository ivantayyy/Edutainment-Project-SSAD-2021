using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;
using System.Threading.Tasks;
using System;

public class LoginManager : MonoBehaviour
{
    //Login variables
    [Header("Login")]
    public InputField emailLoginField;
    public InputField passwordLoginField;
    public Text warningLoginText;
    public Text confirmLoginText;

    //Register variables
    [Header("Register")]
    public Dropdown ddClass;
    public Dropdown ddReg;
    public InputField usernameRegisterField;
    public InputField emailRegisterField;
    public InputField passwordRegisterField;
    public InputField passwordRegisterVerifyField;
    public Text warningRegisterText;

    //LeaderBoard variables
    [Header("LeaderBoard")]
    public GameObject scoreElement;
    public Transform scoreboardCustomContent;
    public Transform scoreboardMultiContent;
    public Transform scoreboardSingleContent;

    [Header("SummaryReport")]
    public GameObject classElement;
    public GameObject studentNameElement;
    public GameObject studentDataElement;
    public Transform classContent;
    public Transform studentNameContent;

    void Start()
    {
        FirebaseManager.CheckFirebaseDependencies();
    }

    //Function for the login button
    public async void LoginButton()
    {
        //Call the login coroutine passing the email and password
        string message = await FirebaseManager.LoginAsync(emailLoginField.text, passwordLoginField.text);
        warningRegisterText.text = message;
    }

    //Dropdown button for Register page
    public void DropDownButtonRegister()
    {
        DropDownCheck(ddReg);
    }
    public void DropDownButtonClass()
    {
        DropDownCheck(ddClass);
    }

    //Visual debug of dropdown button
    private void DropDownCheck(Dropdown dd)
    {
        if (dd.value == 0)
        {
            UnityEngine.Debug.Log("No Dropdown value chosen");
        }
        if (dd.value == 1)
        {
            UnityEngine.Debug.Log("Student chosen");
        }
        if (dd.value == 2)
        {
            UnityEngine.Debug.Log("Teacher chosen");
        }
    }


    //gets acctype from dropdown button
    //3 choices available
    private string getAccType(Dropdown dd)
    {
        string res;
        if (dd.value == 0)
        {
            res = "none";
        }
        else if (dd.value == 1)
        {
            res = "Student";
        }
        else
        {
            res = "Teacher";
        }
        return res;
    }

    private string GetClassSubscribed(Dropdown dd)
    {
        string res;
        if (dd.value == 0)
        {
            res = "none";
        }
        else if (dd.value == 1)
        {
            res = "FS6";
        }
        else if (dd.value == 2)
        {
            res = "FS7";
        }
        else if (dd.value == 3)
        {
            res = "FS8";
        }
        else
        {
            res = "FS9";
        }
        return res;
    }

    //Leaderboard button
    //public void LeaderBoardButton()
    //{
    //    StartCoroutine(LoadLeaderBoardData("customPlayer", scoreboardCustomContent));
    //    StartCoroutine(LoadLeaderBoardData("multiPlayer", scoreboardMultiContent));
    //    StartCoroutine(LoadLeaderBoardData("singlePlayer", scoreboardSingleContent));
    //}

    //button ui to go to summary report page
    public void summaryReportButton()
    {
        LoadClassList();
        UIManager.instance.summaryReportScreen();
    }

    //Signout method
    public void SignOutButton()
    {
        FirebaseManager.SignOut();
        UIManager.instance.LoginScreen();
        ClearRegisterFields();
        ClearLoginFields();
    }
    // clears the login an register fields
    public void ClearLoginFields()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";
        //clear dropdown Account info
    }
    public void ClearRegisterFields()
    {
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordRegisterVerifyField.text = "";
        //clear dropdown Account info
        ddReg.value = 0;
    }

    public async void RegisterButton()
    {
        string acctype = getAccType(ddReg);
        string classSubscribed = GetClassSubscribed(ddClass);

        if (acctype == "none")
        {
            UnityEngine.Debug.Log("No Account inputted in dropdown");
            warningLoginText.text = "Please select a valid account type in the Dropdown menu";
        }
        else if (classSubscribed == "none")
        {
            UnityEngine.Debug.Log("No class inputted in dropdown");
            warningLoginText.text = "Please select a valid account type in the Dropdown menu";
        }
        else if (usernameRegisterField.text == "")
        {
            //If the username field is blank show a warning
            warningRegisterText.text = "Missing Username";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //If the password does not match show a warning
            warningRegisterText.text = "Password Does Not Match!";
        }else if ( await FirebaseManager.CheckUsernameExistsInDatabaseAsync(usernameRegisterField.text))
        {
            warningRegisterText.text = "username already exists";
        }
        else
        {
            UnityEngine.Debug.Log("reached1");
            //Call the register coroutine passing the email, password, and username
            Task<string> messageTask = FirebaseManager.RegisterAsync(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text, acctype, classSubscribed);
            string message = await messageTask;
            UnityEngine.Debug.Log("reached5");
            warningRegisterText.text = message;
        }
    }

    //public static IEnumerator LoadLeaderBoardData(string gameMode, Transform scoreboardContent)
    //{
    //    //Get all the users data ordered by score amount
        
    //}

    //Loads studentNames for the summary report
    //public void LoadStudentNameBtn(string className)
    //{
    //    StartCoroutine(FirebaseManager.LoadStudentNames(className));
    //}

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


    // Update is called once per frame

    void Update()
    {
        
    }
}
