using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Linq;
using UnityEditor;
using System.Threading.Tasks;

using System;
using Firebase.Extensions;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance;
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;
    //public UserDatabaseReference UserDBreference;

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


    void Awake()
    {
        instance = this;
        UnityEngine.Debug.Log("Firebase Manager Awake is called");
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
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
        }else if(dd.value == 1)
        {
            res = "FS6";
        }else if(dd.value == 2)
        {
            res = "FS7";
        }else if (dd.value == 3)
        {
            res = "FS8";
        }
        else
        {
            res = "FS9";
        }
        return res;
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        Debug.Log("Set up Firebase Auth successfully");
    }

    //Function for the login button
    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
        UnityEngine.Debug.Log("afterlgogin");
    }

    //Function for the register button
    public void RegisterButton()
    {
        string acctype = getAccType(ddReg);
        string classSubscribed = GetClassSubscribed(ddClass);
        if (acctype == "none")
        {
            UnityEngine.Debug.Log("No Account inputted in dropdown");
            warningLoginText.text = "Please select a valid account type in the Dropdown menu";
        }else if(classSubscribed == "none")
        {
            UnityEngine.Debug.Log("No class inputted in dropdown");
            warningLoginText.text = "Please select a valid account type in the Dropdown menu";
        }
        else
        {
            //Call the register coroutine passing the email, password, and username
            StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text,acctype,classSubscribed));
        }
    }

    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);
        UnityEngine.Debug.Log("directly after login");
        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            //User is now logged in
            //Now get the result
            User = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            warningLoginText.text = "";
            confirmLoginText.text = "Logged In";

            //redirects to next screen
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene("Main Menu");
            
        }
    }

    //testing use Student and LFd7xpb0fWVY1rC0L9H7AOrplFh2
    public void GetUser(string uid, string acctype)
    {
        
        InitUser currentUser = new InitUser();
        DBreference.Child(acctype).Child(uid).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                UnityEngine.Debug.Log(task.Exception);
            }
            else
            {
                string jsonstring = task.Result.GetRawJsonValue();
                UnityEngine.Debug.Log(jsonstring);
                UnityEngine.Debug.Log(currentUser.classSubscribed);
                currentUser = JsonConvert.DeserializeObject<InitUser>(jsonstring);
                UnityEngine.Debug.Log(currentUser.classSubscribed);
            }
        });                
    }



    //This method adds an intialised user data to firebase database
    private void InitialiseUserData(string acctype, string username, string uid,string classSubscribed)
    {
        InitUser initialuser = new InitUser(acctype, username, uid, classSubscribed);
        var userjson = JsonConvert.SerializeObject(initialuser);

        UnityEngine.Debug.Log(userjson);
        DBreference.Child(acctype).Child(uid).SetRawJsonValueAsync(userjson).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                UnityEngine.Debug.Log(task.Exception);

            }
            else
            {
                UnityEngine.Debug.Log("successfully added " + username + " to database");
            }

        });
    }

    //authentication Registers 
    private IEnumerator Register(string _email, string _password, string _username,string _acctype,string _classSubscribed)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            warningRegisterText.text = "Missing Username";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //If the password does not match show a warning
            warningRegisterText.text = "Password Does Not Match!";
        }
        else
        {
            //checks username exists
            var DBTask = DBreference.Child("Usernames").GetValueAsync();
            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if(DBTask.Exception != null)
            {
                UnityEngine.Debug.Log(DBTask.Exception);
            }
            else
            {
                UnityEngine.Debug.Log("Username database retrieved successfully");
                bool res = DBTask.Result.Child(_username).Exists;
                //if username exists
                if (res == true)
                {
                    warningRegisterText.text = "Username is already taken.Choose another username";
                }
                //if username does not exist continue authentication
                else
                {
                    //Call the Firebase auth signin function passing the email and password
                    var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

                    if (RegisterTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                        FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                        string message = "Register Failed!";
                        switch (errorCode)
                        {
                            case AuthError.MissingEmail:
                                message = "Missing Email";
                                break;
                            case AuthError.MissingPassword:
                                message = "Missing Password";
                                break;
                            case AuthError.WeakPassword:
                                message = "Weak Password";
                                break;
                            case AuthError.EmailAlreadyInUse:
                                message = "Email Already In Use";
                                break;
                        }
                        warningRegisterText.text = message;
                    }
                    else
                    {
                        //User has now been created
                        //Now get the result
                        User = RegisterTask.Result;

                        if (User != null)
                        {
                            //Create a user profile and set the username
                            UserProfile profile = new UserProfile { DisplayName = _username };

                            //Call the Firebase auth update user profile function passing the profile with the username
                            var ProfileTask = User.UpdateUserProfileAsync(profile);

                            //update the username child
                            DBreference.Child("Usernames").Child(_username).SetValueAsync(1);
                            //Exception handling not present
                            //Wait until the task completes
                            yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                            //initialise user data in relational database
                            UnityEngine.Debug.Log(User.UserId + _acctype+_username);

                            //function called here
                            InitialiseUserData(_acctype,_username,User.UserId,_classSubscribed);

                            if (ProfileTask.Exception != null)
                            {
                                //If there are errors handle them
                                Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                                FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                                warningRegisterText.text = "Username Set Failed!";
                            }
                            else
                            {
                                //Username is now set
                                //Now return to login screen
                                UIManager.instance.LoginScreen();
                                warningRegisterText.text = "";
                            }
                        }
                    }
                }
            }
        }
    }



    //Signout method
    public void SignOutButton()
    {
        auth.SignOut();
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


    public void LeaderBoardButton()
    {
        StartCoroutine(LoadLeaderBoardData("customPlayer", scoreboardCustomContent));
        StartCoroutine(LoadLeaderBoardData("multiPlayer", scoreboardMultiContent));
        StartCoroutine(LoadLeaderBoardData("singlePlayer", scoreboardSingleContent));
    }

    private IEnumerator LoadLeaderBoardData(string gameMode, Transform scoreboardContent)
    {
        //Get all the users data ordered by score amount
        var DBTask = DBreference.Child("Student").OrderByChild($"{gameMode}/totalPoints").LimitToLast(10).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            //Destroy any existing scoreboard elements
            foreach (Transform child in scoreboardContent.transform)
            {
                UnityEngine.Debug.Log("reached before transform loop");
                Destroy(child.gameObject);
                UnityEngine.Debug.Log("reached after transform loop");
            }

            //Loop through every users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string username = childSnapshot.Child("username").Value.ToString();
                int score = int.Parse(childSnapshot.Child(gameMode).Child("totalPoints").Value.ToString());
                UnityEngine.Debug.Log(score.ToString());
                UnityEngine.Debug.Log(username);

                //Instantiate new scoreboard elements
                GameObject scoreBoardElement = Instantiate(scoreElement, scoreboardContent);
                scoreBoardElement.GetComponent<ScoreElement>().NewScoreElement(username, score);
            }
        }
    }

    //button ui to go to summary report page
    public void summaryReportButton()
    {
        LoadClassList();
        UIManager.instance.summaryReportScreen();
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

    //loads all student names in class

    public IEnumerator LoadStudentNames(string className)
    {
        UnityEngine.Debug.Log("reached inside LOAdstudentNames function");
        var task = DBreference.Child("Student").OrderByChild("classSubscribed").EqualTo(className).GetValueAsync();
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.Exception != null)
        {
            UnityEngine.Debug.Log(task.Exception);
        }
        else
        {

            DataSnapshot snapshot = task.Result;
            UnityEngine.Debug.Log(task.Result.GetRawJsonValue());
            foreach (Transform child in studentNameContent.transform)
            {
                Destroy(child.gameObject);
                UnityEngine.Debug.Log("Destroyed a child");
            }
            //Need to instantiate prefab dynamically
            foreach (DataSnapshot student in snapshot.Children.Reverse<DataSnapshot>())
            {
                string username = student.Child("username").Value.ToString();
                UnityEngine.Debug.Log(username);
                GameObject nameBoardElement = Instantiate(studentNameElement, this.studentNameContent);
                nameBoardElement.GetComponent<StudentNameElement>().NewStudentNameElement(username);
            }
        }
    }

    public void LoadStudentNameBtn(string className)
    {
        StartCoroutine(LoadStudentNames(className));
    }

    /*
     * 
     * retrieving and getting data
     */

    // for updating username in auth
    //private IEnumerator UpdateUsernameAuth(string _username)
    //{
    //    //Create a user profile and set the username
    //    UserProfile profile = new UserProfile { DisplayName = _username };

    //    //Call the Firebase auth update user profile function passing the profile with the username
    //    var ProfileTask = User.UpdateUserProfileAsync(profile);
    //    //Wait until the task completes
    //    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

    //    if (ProfileTask.Exception != null)
    //    {
    //        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
    //    }
    //    else
    //    {
    //        //Auth username is now updated
    //    }
    //}


    //// for updating username in scoreboard
    //private IEnumerator UpdateUsernameDatabase(string _username)
    //{
    //    //Set the currently logged in user username in the database
    //    var DBTask = DBreference.Child("users").Child(User.UserId).Child("username").SetValueAsync(_username);

    //    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

    //    if (DBTask.Exception != null)
    //    {
    //        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    //    }
    //    else
    //    {
    //        //Database username is now updated
    //    }
    //}


    ////TODO to cheange
    ////Function for the save button
    //public void SaveDataButton()
    //{
    //    StartCoroutine(UpdateUsernameAuth(usernameField.text));
    //    StartCoroutine(UpdateUsernameDatabase(usernameField.text));

    //    StartCoroutine(UpdateXp(int.Parse(xpField.text)));
    //    StartCoroutine(UpdateKills(int.Parse(killsField.text)));
    //    StartCoroutine(UpdateDeaths(int.Parse(deathsField.text)));
    //}

    //private IEnumerator UpdateXp(int _xp)
    //{
    //    //Set the currently logged in user xp
    //    var DBTask = DBreference.Child("users").Child(User.UserId).Child("xp").SetValueAsync(_xp);

    //    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

    //    if (DBTask.Exception != null)
    //    {
    //        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    //    }
    //    else
    //    {
    //        //Xp is now updated
    //    }
    //}

    //private IEnumerator UpdateKills(int _kills)
    //{
    //    //Set the currently logged in user kills
    //    var DBTask = DBreference.Child("users").Child(User.UserId).Child("kills").SetValueAsync(_kills);

    //    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

    //    if (DBTask.Exception != null)
    //    {
    //        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    //    }
    //    else
    //    {
    //        //Kills are now updated
    //    }
    //}

    //private IEnumerator UpdateDeaths(int _deaths)
    //{
    //    //Set the currently logged in user deaths
    //    var DBTask = DBreference.Child("users").Child(User.UserId).Child("deaths").SetValueAsync(_deaths);

    //    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

    //    if (DBTask.Exception != null)
    //    {
    //        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    //    }
    //    else
    //    {
    //        //Deaths are now updated
    //    }
    //}

    //private IEnumerator LoadUserData()
    //{
    //    //Get the currently logged in user data
    //    var DBTask = DBreference.Child("Student").Child(User.UserId).GetValueAsync();

    //    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

    //    if (DBTask.Exception != null)
    //    {
    //        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    //    }
    //    else if (DBTask.Result.Value == null)
    //    {

    //    }
    //    else
    //    {
    //        //Data has been retrieved
    //        DataSnapshot snapshot = DBTask.Result;

    //        xpField.text = snapshot.Child("xp").Value.ToString();
    //        killsField.text = snapshot.Child("kills").Value.ToString();
    //        deathsField.text = snapshot.Child("deaths").Value.ToString();
    //    }
    //}



}
