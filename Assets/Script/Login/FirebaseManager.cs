using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;
using UnityEngine;
using System.Runtime.Remoting.Messaging;

public static class FirebaseManager
{
    //Firebase variables
    public static DependencyStatus dependencyStatus;
    public static FirebaseAuth auth;
    public static FirebaseUser User;
    public static DatabaseReference DBreference;
    //public UserDatabaseReference UserDBreference;

    

    //Initialises Firebase
    private static void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        Debug.Log("Set up Firebase Auth successfully");
    }

    public static void CheckFirebaseDependencies()
    {
        UnityEngine.Debug.Log("Firebase Manager Awake is called");
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                FirebaseManager.InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    async public static Task<string> LoginAsync(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        //Wait until the task completes
        UnityEngine.Debug.Log("Login reached 1");
        Task<FirebaseUser> LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        FirebaseUser User =await LoginTask;
        UnityEngine.Debug.Log("Login reached 2");

        string message;
        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            message = "Login Failed!";
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
            UnityEngine.Debug.Log("Login reached 3");

        }
        else
        {
            //User is now logged in
            //Now get the result
            UnityEngine.Debug.Log("Login reached 4");
            User = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            message = "Logged In";

            //redirects to next screen
            SceneManager.LoadScene("Main Menu");
            // wait for 2 seconds
            new WaitForSeconds(2);
            UnityEngine.Debug.Log("Login reached 5");

        }
        UnityEngine.Debug.Log("Login reached 6");
        return message;
    }

    public static void SignOut()
    {
        auth.SignOut();
    }

    //testing use Student and LFd7xpb0fWVY1rC0L9H7AOrplFh2
    // TODO test this function
    async public static Task<InitUser> GetUser(string uid, string acctype)
    {
        
        InitUser currentUser = new InitUser();
        Task<DataSnapshot> task = DBreference.Child(acctype).Child(uid).GetValueAsync();
        DataSnapshot snapshot = await task;
        if (task.IsFaulted)
        {
            UnityEngine.Debug.Log(task.Exception);
            currentUser = null;
        }
        else
        {
            string jsonstring = task.Result.GetRawJsonValue();
            UnityEngine.Debug.Log(jsonstring);
            UnityEngine.Debug.Log(currentUser.classSubscribed);
            currentUser = JsonConvert.DeserializeObject<InitUser>(jsonstring);
            UnityEngine.Debug.Log(currentUser.classSubscribed)
        }
        return currentUser;
    }

    //public static void GetLeaderBoardFromDatabase(string gameMode)
    //{
    //    var DBTask = DBreference.Child("Student").OrderByChild($"{gameMode}/totalPoints").LimitToLast(10).GetValueAsync();

    //    yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

    //    if (DBTask.Exception != null)
    //    {
    //        Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
    //    }
    //    else
    //    {
    //        //Data has been retrieved
    //        DataSnapshot snapshot = DBTask.Result;

    //        //Destroy any existing scoreboard elements
    //        foreach (Transform child in scoreboardContent.transform)
    //        {
    //            UnityEngine.Debug.Log("reached before transform loop");
    //            Destroy(child.gameObject);
    //            UnityEngine.Debug.Log("reached after transform loop");
    //        }

    //        //Loop through every users UID
    //        foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
    //        {
    //            string username = childSnapshot.Child("username").Value.ToString();
    //            int score = int.Parse(childSnapshot.Child(gameMode).Child("totalPoints").Value.ToString());
    //            UnityEngine.Debug.Log(score.ToString());
    //            UnityEngine.Debug.Log(username);

    //            //Instantiate new scoreboard elements
    //            GameObject scoreBoardElement = Instantiate(scoreElement, scoreboardContent);
    //            scoreBoardElement.GetComponent<ScoreElement>().NewScoreElement(username, score);
    //        }
    //    }
    //}


    //This method adds an intialised user data to firebase database
    private static void InitialiseUserData(string acctype, string username, string uid,string classSubscribed)
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

    //Helper function for checking username exist in database for registration
   async public static Task<bool> CheckUsernameExistsInDatabaseAsync(string _username)
    {
        Task<DataSnapshot> usernameTask = await DBreference.Child("Usernames").GetValueAsync().ContinueWith(t=>t);
        bool res = usernameTask.Result.Child(_username).Exists;
        return res;
    }

    //authentication Registers 
    async public static Task<string> RegisterAsync(string _email, string _password, string _username,string _acctype,string _classSubscribed)
    {
        UnityEngine.Debug.Log("Reached2");

        //Call the Firebase auth signin function passing the email and password
        Task<FirebaseUser> RegisterTask = await auth.CreateUserWithEmailAndPasswordAsync(_email, _password).ContinueWith(t=>t);
        //supposed to wait for Register Task to finish
        string message = "";
        if (RegisterTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
            FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

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
        }
        else
        {
            //User has now been created
            //Now get the result
            UnityEngine.Debug.Log("Reached3");

            User = RegisterTask.Result;
            UnityEngine.Debug.Log("Reached4");

            if (User != null)
            {
                //Create a user profile and set the username
                UserProfile profile = new UserProfile { DisplayName = _username };

                //Call the Firebase auth update user profile function passing the profile with the username
                Task ProfileTask = await User.UpdateUserProfileAsync(profile).ContinueWith(t=>t);

                //update the username child
                Task task = await DBreference.Child("Usernames").Child(_username).SetValueAsync(1).ContinueWithOnMainThread(t => t);
                if (task.IsFaulted)
                {
                    UnityEngine.Debug.Log("Task is faulted 2");
                }
                else
                {
                    UnityEngine.Debug.Log(User.UserId + _acctype + _username);
                    InitialiseUserData(_acctype, _username, User.UserId, _classSubscribed);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        message = "Username Set Failed!";
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        UIManager.instance.LoginScreen();
                        message = "";
                    }
                }
            }
            else
            {
                message = "registration success but User is null";
            }
        }
        return message;
    } 
}
