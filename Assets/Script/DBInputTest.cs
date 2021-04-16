using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;


namespace Assets
{
    public class DBInputTest : MonoBehaviour
    {
        public static DependencyStatus dependencyStatus;
        public static FirebaseAuth auth;
        public static FirebaseUser User;
        public static DatabaseReference DBreference;
        public static List<string> userID = new List<string>();

        public static string DatabaseEndPoint = "https://fir-auth-9c8cd-default-rtdb.firebaseio.com/";

        private void Start()
        {
            CheckFirebaseDependencies();
        }
        public static void CheckFirebaseDependencies()
        {
            UnityEngine.Debug.Log("Firebase Manager Awake is called");
            // Check that all of the necessary dependencies for Firebase are present on the system
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    Debug.Log("Dependencies available for use");
                    // If they are avalible Initialize Firebase
                    auth = FirebaseAuth.DefaultInstance;
                    DBreference = FirebaseDatabase.DefaultInstance.RootReference;
                }
                else
                {
                    Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
                }
            });
        }
        async public void inputStudent()
        {
            await DBreference.Child("Classes").Child("TestClass").RemoveValueAsync();
            List<string> email = new List<string>() { "test1@gmail.com", "test2@gmail.com", "test3@gmail.com" };
            string password = "password123";
            List<string> username = new List<string>() { "test1", "test2", "test3" };
            for(int i=0;i<3;i++)
            {
                Debug.Log("email: " + email[i] + " " + password + " " + username[i]);
                Task<string> messageTask = RegisterAsync(email[i], password, username[i], "Student", "TestClass");
                string message = await messageTask;
                Debug.Log(message);
            }
            
        }

        async public void inputScore()
        {
            System.Random rnd = new System.Random();
            //userID.Add("bo0mgueAnCNUbcWj5rJ9vS7nAjm1");
            List<string> mode = new List<string>() { "singlePlayer", "multiPlayer", "customPlayer" };
            foreach (string uid in userID)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int i = 1; i < 16; i++)
                    {
                        int time = rnd.Next(30, 300);
                        int points = rnd.Next(3, 16);
                        await updateScoreOnDatabaseAsync(mode[j], uid, i, time, points);
                    }
                }

            }
            

        }

        async public static Task<string> RegisterAsync(string _email, string _password, string _username, string _acctype, string _classSubscribed)
        {
            UnityEngine.Debug.Log("Reached RegisterAsync Method");

            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            await RegisterTask;
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
                UnityEngine.Debug.Log("Register suuccess");
                FirebaseUser User = RegisterTask.Result;
                //Create a user profile and set the username
                //store username in firebase
                Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile { DisplayName = _username };

                //Call the Firebase auth update user profile function passing the profile with the username
                Task ProfileTask = User.UpdateUserProfileAsync(profile);

                //update the username child
                Task AddUsernameTask = DBreference.Child("Usernames").Child(_username).SetValueAsync(1);

                await Task.WhenAll(ProfileTask, AddUsernameTask);


                UnityEngine.Debug.Log(User.UserId + _acctype + _username);
                userID.Add(User.UserId);


                message = "Successfully Registered";
                
                await InitialiseUserData(_acctype, _username, User.UserId, _classSubscribed);

            }
            return message;
        }


        private async static Task InitialiseUserData(string acctype, string username, string uid, string classSubscribed)
        {
            string userjson;
            if (acctype == "Teacher")
            {
                Teacher teacher = new Teacher(acctype, username, uid);
                userjson = JsonConvert.SerializeObject(teacher);
            }
            else
            {
                InitUser student = new InitUser(acctype, username, uid, classSubscribed);
                userjson = JsonConvert.SerializeObject(student);
                await subscribeClass(classSubscribed, uid);
            }

            //UnityEngine.Debug.Log(userjson);
            var task = DBreference.Child(acctype).Child(uid).SetRawJsonValueAsync(userjson);
            await task;
            if (task.IsFaulted)
            {
                UnityEngine.Debug.Log(task.Exception);

            }
            else
            {
                UnityEngine.Debug.Log("successfully added " + username + " to database");
            }
        }
        private async static Task subscribeClass(string className, string uid)
        {
            List<string> ListOfSubscribers = new List<string>();
            var classRef = DBreference.Child("Classes").Child(className).GetValueAsync();
            DataSnapshot datasnapshot = await classRef;
            string jsonObject = datasnapshot.GetRawJsonValue();
            ListOfSubscribers = JsonConvert.DeserializeObject<List<string>>(jsonObject);
            Debug.Log(jsonObject);
            ListOfSubscribers.Add(uid);
            string toUpdate = JsonConvert.SerializeObject(ListOfSubscribers);
            var updateTask = DBreference.Child("Classes").Child(className).SetRawJsonValueAsync(toUpdate);
            await updateTask;
            if (updateTask.Exception != null)
            {
                Debug.Log("updatetask failed");
            }
            else
            {
                Debug.Log("update Succesful");
            }

        }

        async public static Task updateScoreOnDatabaseAsync(string gamemode, string userid, int justFinishedlevel, float new_timeTaken, float new_points)
        {
            UnityEngine.Debug.Log("reached updateScoreOnDatabaseAsync");
            UnityEngine.Debug.Log(userid);
            Debug.Log("gamemode: " + gamemode);
            Scores DBScore = new Scores();
            var scoreTask = DBreference.Child("Student").Child(userid).Child(gamemode).GetValueAsync();
            DataSnapshot playerScore = await scoreTask;
            UnityEngine.Debug.Log("Task returned successfully");

            if (scoreTask.IsFaulted)
            {
                Debug.Log("updateScoreOnDatabaseAsync is fked");
            }
            else
            {
                string DBPlayerScorejsonString = playerScore.GetRawJsonValue();
                // UnityEngine.Debug.Log(DBPlayerScorejsonString);

                DBScore = JsonConvert.DeserializeObject<Scores>(DBPlayerScorejsonString);

                //increase attempts by one
                int curSubStage = DBScore.curSubstage;
                float totalPoints = DBScore.totalPoints;
                List<int> attempts = DBScore.attempts;
                List<float> points = DBScore.points;
                List<float> timeTaken = DBScore.timeTaken;

                // Debug
                //foreach (int attempt in attempts)
                //{
                //    Debug.Log($"Attempt for updating function {attempt}");
                //}
                //foreach (int point in points)
                //{
                //    Debug.Log($"points for updating function {point}");
                //}
                //foreach (int time in timeTaken)
                //{
                //    Debug.Log($"timetaken for stage for updating function {time}");
                //}

                Debug.Log($"Current stage in database is {curSubStage}");
                // If player has cleared his max substage
                if (justFinishedlevel == curSubStage)
                {
                    Debug.Log($"User cleared his max substage {curSubStage}");
                    // add  1 attempt to curr stage then add 0 to new stage
                    DBScore.attempts[justFinishedlevel - 1] += 1;
                    // Add another element for new stage
                    DBScore.attempts.Add(0);

                    DBScore.points[justFinishedlevel - 1] = new_points;
                    DBScore.points.Add(0);

                    DBScore.timeTaken[justFinishedlevel - 1] = new_timeTaken;
                    DBScore.timeTaken.Add(0);

                    //foreach(int attempt in DBScore.attempts)
                    //{
                    //    Debug.Log(attempt);
                    //}
                    //calculate total points
                    float new_totalPoints = 0;
                    foreach (float StagePoint in points)
                    {
                        new_totalPoints += StagePoint;
                    }
                    DBScore.totalPoints = new_totalPoints;
                    Debug.Log($"Total Points for this user is {DBScore.totalPoints}");
                    // increment curSubstage to unlock next level
                    DBScore.curSubstage += 1;
                }
                else // means replaying a previously cleared level
                {
                    // if achieved new highscore
                    if (new_points > points[justFinishedlevel - 1])
                    {
                        // increase attempts for that level by 1
                        DBScore.attempts[justFinishedlevel - 1] = DBScore.attempts[justFinishedlevel - 1] + 1;

                        // update lower values for timetaken
                        DBScore.timeTaken[justFinishedlevel - 1] = new_timeTaken;

                        // update new points
                        DBScore.points[justFinishedlevel - 1] = new_points;

                        // update new total points
                        float newTotalPoints = 0;
                        foreach (float stagePoint in points)
                        {
                            newTotalPoints += stagePoint;
                        }
                        DBScore.totalPoints = newTotalPoints;


                    }
                    else// if never achieve new highscore
                    {
                        DBScore.attempts[justFinishedlevel - 1] = DBScore.attempts[justFinishedlevel - 1] + 1;

                    }
                }

                // update the database
                string updatedUserScore = JsonConvert.SerializeObject(DBScore);
                var Task = DBreference.Child("Student").Child(userid).Child(gamemode).SetRawJsonValueAsync(updatedUserScore);
                await Task;
                if (Task.IsFaulted)
                {
                    Debug.Log("Unable to update user");
                }
                else
                {
                    Debug.Log("User's score is successfully updated");
                }
            }
        }



    }
}