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
using UnityEngine.PlayerLoop;
using UnityEngine.SocialPlatforms.Impl;
using System.Dynamic;
using System.Linq;

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

    //Helper function for checking dependencies 
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

    //login function
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

    //Signout function
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
            UnityEngine.Debug.Log(currentUser.classSubscribed);
        }
        return currentUser;
    }

    //set Score
    async public static void updateScoreOnDatabaseAsync(string gamemode, string userid, int new_level,float new_timeTaken,float new_points)
    {
        UnityEngine.Debug.Log("reached a");
        UnityEngine.Debug.Log(userid);

        Scores DBScore = new Scores();
        //Scores finalScore = new Scores();
        //implement max function next time
        Task<DataSnapshot> scoreTask = DBreference.Child("Student").Child(userid).Child(gamemode).GetValueAsync();
        UnityEngine.Debug.Log("reached b");
        DataSnapshot playerScore = await scoreTask;
        //Do eXception handling
        string DBPlayerScorejsonString = playerScore.GetRawJsonValue();
        UnityEngine.Debug.Log(DBPlayerScorejsonString);

        DBScore = JsonConvert.DeserializeObject<Scores>(DBPlayerScorejsonString);

        //increase attempts by obe
        int curSubStage = DBScore.curSubstage;
        float totalPoints = DBScore.totalPoints;
        List<int> attempts = DBScore.attempts;
        List<float> points = DBScore.points;
        List<float> timeTaken = DBScore.timeTaken;
        
        
        //extract old stats from db for this try at this level
        

        //If player has cleared his max substage
        if (new_level == DBScore.curSubstage)
        {
            DBScore.attempts.Add(1);
            DBScore.points.Add(new_points);
            DBScore.timeTaken.Add(new_timeTaken);

            //calculate total points
            float new_totalPoints = 0;
            foreach(float StagePoint in points)
            {
                new_totalPoints += StagePoint;
            }
            DBScore.totalPoints = new_totalPoints;

            //increment curSubstage to unlock next level
            DBScore.curSubstage += 1;
        }else // means replaying a previously cleared level
        {
            //if achieved new highscore
            if (new_points < points[new_level - 1])
            {
                //increase attempts for that level by 1
                DBScore.attempts[new_level - 1] = DBScore.attempts[new_level - 1] + 1;

                //update lower values for timetaken
                DBScore.timeTaken[new_level - 1] = new_timeTaken;

                //update new points
                DBScore.points[new_level - 1] = new_points;

                //update new total points
                float newTotalPoints = 0;
                foreach(float stagePoint in points)
                {
                    newTotalPoints += stagePoint;
                }
                DBScore.totalPoints = newTotalPoints;


            }
            else//if never achieve new highscore
            {
                DBScore.attempts[new_level - 1] = DBScore.attempts[new_level - 1] + 1;

            }

            //update the database
            string updatedUserScore = JsonConvert.SerializeObject(DBScore);
            await DBreference.Child("Student").Child(userid).Child(gamemode).SetRawJsonValueAsync(updatedUserScore);
        }

        //DEBUG
        UnityEngine.Debug.Log(curSubStage);
        UnityEngine.Debug.Log(totalPoints);
        UnityEngine.Debug.Log(attempts[new_level-1]);
        UnityEngine.Debug.Log(points[new_level-1]);
        UnityEngine.Debug.Log(timeTaken[new_level-1]);
        UnityEngine.Debug.Log("reached z");



        //update new Score
        //string finalScores = JsonConvert.SerializeObject(finalScores);
        //DBreference.Child("Student").Child(userid).Child(userid).Child(gamemode).SetRawJsonValueAsync(finalScores);
    }

    //get users current max level
    async public static Task<int> getUserMaxLevelReachedAsync(string gamemode)
    {
        UnityEngine.Debug.Log("reached");
        string uid = PhotonNetwork.player.UserId;
        DataSnapshot snapshot = await DBreference.Child("Student").Child(uid).Child(gamemode).Child("curSubstage").GetValueAsync();
        int maxLevelReached = JsonConvert.DeserializeObject<int>(snapshot.GetRawJsonValue());;

        return maxLevelReached;
    }

    async public static Task<List<InitUser>> GetLeaderBoardFromDatabase(string gameMode)
    {
        DataSnapshot LeaderBoardSnapShot = await DBreference.Child("Student").OrderByChild($"{gameMode}/totalPoints").LimitToLast(10).GetValueAsync();
        //Loop through every users UID
        List<InitUser> LeaderBoardUsers = new List<InitUser>();
        UnityEngine.Debug.Log(LeaderBoardSnapShot.ChildrenCount);
        foreach (DataSnapshot childSnapshot in LeaderBoardSnapShot.Children.Reverse<DataSnapshot>())
        {
            string childJsonString = childSnapshot.GetRawJsonValue();
            UnityEngine.Debug.Log(childJsonString);
            LeaderBoardUsers.Add(JsonConvert.DeserializeObject<InitUser>(childJsonString));
        }
        UnityEngine.Debug.Log("Debug hhere"+LeaderBoardUsers.Count.ToString());
        return LeaderBoardUsers;
    }




    //Helper function for checking username exist in database for registration
    async public static Task<bool> CheckUsernameExistsInDatabaseAsync(string _username)
    {
        Task<DataSnapshot> usernameTask = await DBreference.Child("Usernames").GetValueAsync().ContinueWith(t=>t);
        bool res = usernameTask.Result.Child(_username).Exists;
        return res;
    }

    //This method adds an intialised user data to firebase database
    private static void InitialiseUserData(string acctype, string username, string uid, string classSubscribed)
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
                Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile { DisplayName = _username };

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
