using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
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
                Debug.Log("Dependencies available for use");
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
        string message = "";

        //Call the Firebase auth signin function passing the email and password
        //Wait until the task completes
        //UnityEngine.Debug.Log("Login reached 1");
        Debug.Log("reached0");

        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        try
        {
            await LoginTask;
        }
        catch (Exception err)
        {
            Debug.Log("reached3");
            Debug.Log($"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            Debug.Log("LoginTask in FirebaseManager.LoginAsync fked up");
            message = $"Failed to register task with {LoginTask.Exception}";


            //If there are errors handle them
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
            Debug.Log("Result: "+message);
            return message;
        }

        if (LoginTask.Exception == null)
        {
            User = LoginTask.Result;
            //User is now logged in
            //Now get the result
            //UnityEngine.Debug.Log("Login reached 3");
            //User = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            message = "Logged In";
        }
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
        UnityEngine.Debug.Log("Reached GetUser() Function");
        InitUser currentUser;
        var task = DBreference.Child(acctype).Child(uid).GetValueAsync();
        DataSnapshot userSnapShot = await task;
        string userjsonstring = userSnapShot.GetRawJsonValue();
        UnityEngine.Debug.Log($"Json string returned in GetUser() with: {userjsonstring}");

        currentUser = JsonConvert.DeserializeObject<InitUser>(userjsonstring);
        UnityEngine.Debug.Log($"Json string deserialised. Returning User to StudentSummaryREport Manager");
        return currentUser;
    }
    //checks if User is teacher or student

    async public static Task<bool> isTeacher()
    {
        var Task = DBreference.Child("Teacher").Child(User.UserId).GetValueAsync();
        DataSnapshot existsDataSnapshot = await Task;
        bool exist = existsDataSnapshot.Exists;

        Debug.Log($"The {User.UserId} is a teacher? {exist} ");
        //if (Task.IsFaulted)
        //{
        //    Debug.Log("isTeachher function is fked");
        //}

        return exist;
        
    }
    

    //updates Score
    async public static Task updateScoreOnDatabaseAsync(string gamemode, string userid, int justFinishedlevel,float new_timeTaken,float new_points)
    {
        UnityEngine.Debug.Log("reached a");
        UnityEngine.Debug.Log(userid);

        Scores DBScore = new Scores();
        //Scores finalScore = new Scores();
        //implement max function next time
        var scoreTask = DBreference.Child("Student").Child(userid).Child(gamemode).GetValueAsync();
        DataSnapshot playerScore = await scoreTask;

        if (scoreTask.IsFaulted)
        {
            Debug.Log("updateScoreOnDatabaseAsync is fked");
        }
        else
        {
            string DBPlayerScorejsonString = playerScore.GetRawJsonValue();
            UnityEngine.Debug.Log(DBPlayerScorejsonString);

            DBScore = JsonConvert.DeserializeObject<Scores>(DBPlayerScorejsonString);

            //increase attempts by obe
            int curSubStage = DBScore.curSubstage;
            float totalPoints = DBScore.totalPoints;
            List<int> attempts = DBScore.attempts;
            List<float> points = DBScore.points;
            List<float> timeTaken = DBScore.timeTaken;

            //Debug
            foreach (int attempt in attempts)
            {
                Debug.Log($"Attempt for updating function {attempt}");
            }
            foreach (int point in points)
            {
                Debug.Log($"points for updating function {point}");
            }
            foreach (int time in timeTaken)
            {
                Debug.Log($"timetaken for stage for updating function {time}");
            }

            //If player has cleared his max substage
            if (justFinishedlevel == DBScore.curSubstage)
            {
                //add  1 attempt to curr stage then add 0 to new stage
                DBScore.attempts[justFinishedlevel - 1] = DBScore.attempts[justFinishedlevel - 1] + 1;
                DBScore.attempts.Add(0);
                DBScore.points.Add(new_points);
                DBScore.timeTaken.Add(new_timeTaken);

                //calculate total points
                float new_totalPoints = 0;
                foreach (float StagePoint in points)
                {
                    new_totalPoints += StagePoint;
                }
                DBScore.totalPoints = new_totalPoints;

                //increment curSubstage to unlock next level
                DBScore.curSubstage += 1;
            }
            else // means replaying a previously cleared level
            {
                //if achieved new highscore
                if (new_points < points[justFinishedlevel - 1])
                {
                    //increase attempts for that level by 1
                    DBScore.attempts[justFinishedlevel - 1] = DBScore.attempts[justFinishedlevel - 1] + 1;

                    //update lower values for timetaken
                    DBScore.timeTaken[justFinishedlevel - 1] = new_timeTaken;

                    //update new points
                    DBScore.points[justFinishedlevel - 1] = new_points;

                    //update new total points
                    float newTotalPoints = 0;
                    foreach (float stagePoint in points)
                    {
                        newTotalPoints += stagePoint;
                    }
                    DBScore.totalPoints = newTotalPoints;


                }
                else//if never achieve new highscore
                {
                    DBScore.attempts[justFinishedlevel - 1] = DBScore.attempts[justFinishedlevel - 1] + 1;

                }

                //update the database
                string updatedUserScore = JsonConvert.SerializeObject(DBScore);
                DBreference.Child("Student").Child(userid).Child(gamemode).SetRawJsonValueAsync(updatedUserScore);

                //DEBUG
                UnityEngine.Debug.Log(curSubStage);
                UnityEngine.Debug.Log(totalPoints);
                UnityEngine.Debug.Log(attempts[justFinishedlevel - 1]);
                UnityEngine.Debug.Log(points[justFinishedlevel - 1]);
                UnityEngine.Debug.Log(timeTaken[justFinishedlevel - 1]);
                UnityEngine.Debug.Log("reached z");
            }
        }



        //update new Score
        //string finalScores = JsonConvert.SerializeObject(finalScores);
        //DBreference.Child("Student").Child(userid).Child(userid).Child(gamemode).SetRawJsonValueAsync(finalScores);
    }


    //get users current max level
    async public static Task<int> getUserMaxLevelReachedAsync(string gamemode)
    {
        UnityEngine.Debug.Log("reached");
        string uid = PhotonNetwork.player.UserId;
        var snapshotTask = DBreference.Child("Student").Child(uid).Child(gamemode).Child("curSubstage").GetValueAsync();
        DataSnapshot snapshot = await snapshotTask;
        int maxLevelReached = JsonConvert.DeserializeObject<int>(snapshot.GetRawJsonValue());;
        Debug.Log($"The maxLevelReached that was returned from database for {gamemode} is {maxLevelReached}");
        return maxLevelReached;
    }

    async public static Task<List<InitUser>> GetLeaderBoardFromDatabase(string gameMode)
    {
        List<InitUser> LeaderBoardUsers = new List<InitUser>();
        Debug.Log("Loading LeaderBoardSnapShot Task in FirebaseManager");
        var LeaderBoardSnapShotTask = DBreference.Child("Student").OrderByChild($"{gameMode}/totalPoints").LimitToLast(10).GetValueAsync();
        DataSnapshot LeaderBoardSnapShot = await LeaderBoardSnapShotTask;
        if (LeaderBoardSnapShotTask.IsFaulted)
        {
            Debug.Log("LeaderBoardSnapShot Task in FirebaseManager fked up");
        }
        else
        {
            //Loop through every users UID
            UnityEngine.Debug.Log($"The Number of students returned from firebase in {gameMode} leaderboard is {LeaderBoardSnapShot.ChildrenCount}");
            foreach (DataSnapshot childSnapshot in LeaderBoardSnapShot.Children.Reverse<DataSnapshot>())
            {
                string childJsonString = childSnapshot.GetRawJsonValue();
                UnityEngine.Debug.Log(childJsonString);
                InitUser LeaderBoardUser = JsonConvert.DeserializeObject<InitUser>(childJsonString);
                LeaderBoardUsers.Add(LeaderBoardUser);
            }
        }
        return LeaderBoardUsers;
    }

    async public static Task<Dictionary<string ,string>> LoadStudentNamesAsync(string className)
    {
        Debug.Log("Sucessfully reached LoadStudentNamesAsync which is for Summary Report in FirebaseManager");
        Dictionary<string, string> StudentsInfo = new Dictionary<string, string>();

        var StudentsInClassSnapshotTask = DBreference.Child("Student").OrderByChild("classSubscribed").EqualTo(className).GetValueAsync();
        DataSnapshot StudentsInClassSnapShot = await StudentsInClassSnapshotTask;

        if (StudentsInClassSnapshotTask.IsFaulted)
        {
            Debug.Log("The Task to get Students for summary report page is fked");
        }
        else
        {
            UnityEngine.Debug.Log($"Students successfully loaded from Database \n There are{StudentsInClassSnapShot.ChildrenCount} Students in class {className}");

            Debug.Log($"There are {StudentsInClassSnapShot.ChildrenCount} in class {className}");
            foreach (DataSnapshot childSnapshot in StudentsInClassSnapShot.Children.Reverse<DataSnapshot>())
            {
                string childJsonString = childSnapshot.GetRawJsonValue();
                UnityEngine.Debug.Log(childJsonString);
                InitUser child = JsonConvert.DeserializeObject<InitUser>(childJsonString);
                StudentsInfo.Add(child.id, child.username);
            }
            Debug.Log("Completed Json Conversion. Returning control to SummaryReportManager");
        }
        return StudentsInfo;
    }




    //Helper function for checking username exist in database for registration
    async public static Task<bool> CheckUsernameExistsInDatabaseAsync(string _username)
    {
        var usernameTask = DBreference.Child("Usernames").GetValueAsync();
        DataSnapshot username = await usernameTask;
        bool hasUsername = username.Child(_username).Exists;
        Debug.Log($"Does username exist in database? {hasUsername}");
        return hasUsername;
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
