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
using System.Linq;
using Proyecto26;
using System.Net;
using RSG;

namespace Assets
{
    /**
     * FirebaseManager handles database functions.
     */
    public static class FirebaseManager
    {
        // Firebase variables
        public static DependencyStatus dependencyStatus;
        public static FirebaseAuth auth;
        public static FirebaseUser User;
        public static DatabaseReference DBreference;

        public static string DatabaseEndPoint = "https://fir-auth-9c8cd-default-rtdb.firebaseio.com/";

        /**
         * Function to register Student
         * @param _email contains email of user
         * @param _password contains password of account
         * @param _username contains username of user
         * @param _acctype contains type of account being registered
         * @param _classSubscribed contains class that user is subscribing to
         */
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

                if (ProfileTask.Exception != null)
                {
                    //If there are errors handle them
                    Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                    FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                    message = "Username Set Failed!";
                }
                else if (AddUsernameTask.Exception != null)
                {
                    UnityEngine.Debug.Log("Unable to Add UserName to Database");
                    message = "Username Set Failed!";
                }
                else
                {
                    //Username is now set
                    //Now return to login screen
                    LoginUIManager.instance.LoginScreen();
                    message = "Successfully Registered";
                }
                await InitialiseUserData(_acctype, _username, User.UserId, _classSubscribed);

            }
            return message;
        }

        /**
         * Used to get a list of all students subscribed in the class
         * @param className contains name of class from where to fetch the list from
         */
        public async static Task<List<string>> getAllUsersInClass(string className)
        {
            List<string> AllUsersList = new List<string>();
            var Task = DBreference.Child("Classes").Child(className).GetValueAsync();
            DataSnapshot AllUsersSnapShot = await Task;
            string AllUsersListJsonString = AllUsersSnapShot.GetRawJsonValue();
            //Need Add Exception Handling
            if (Task.IsFaulted)
            {
                Debug.Log(Task.Exception);
            }
            else
            {
                AllUsersList = JsonConvert.DeserializeObject<List<string>>(AllUsersListJsonString);
            }
            //Debug
            foreach (string user in AllUsersList)
            {
                Debug.Log(user);
            }

            return AllUsersList;
        }

        /**
         * Login Methods
         * @param _email contains email of user
         * @param _password contains password of user
         */
        async public static Task<string> LoginAsync(string _email, string _password)
        {
            string message = "";

            // Call the Firebase auth signin function passing the email and password
            // Wait until the task completes
            // UnityEngine.Debug.Log("Login reached 1");
            Debug.Log("reached0");

            var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
            try
            {
                await LoginTask;
            }
            catch (Exception err)
            {
                FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
                Debug.Log("LoginTask in FirebaseManager.LoginAsync fked up");
                message = $"Failed to register task with {LoginTask.Exception}";


                // If there are errors handle them
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
                Debug.Log("Result: " + message);
                return message;
            }

            if (LoginTask.Exception == null)
            {
                User = LoginTask.Result;
                // User is now logged in
                // Now get the result
                // UnityEngine.Debug.Log("Login reached 3");
                // User = LoginTask.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
                message = "Logged In";
            }
            return message;
        }

        /**
         * Run on login page Start() for Login Initialises Firebase
         */
        private static void InitializeFirebase()
        {
            Debug.Log("Setting up Firebase Auth");
            //Set the authentication instance object
            auth = FirebaseAuth.DefaultInstance;
            DBreference = FirebaseDatabase.DefaultInstance.RootReference;
            Debug.Log("Set up Firebase Auth successfully");
        }

        /** 
         * Run on LoginPage at STart() to Check dependencies 
         */
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
                    FirebaseManager.InitializeFirebase();
                }
                else
                {
                    Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
                }
            });
        }

        /**
         * Signout function 
         */
        public static void SignOut()
        {
            auth.SignOut();
        }

        /**
         * Function gets a Student User with uid and accType
         * @param uid Student user id
         * @param acctype User's account type
         */
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

        /**
         * Function too check if currenUser is a teacher
         */
        async public static Task<bool> isTeacher()
        {
            var Task = DBreference.Child("Teacher").Child(User.UserId).GetValueAsync();
            DataSnapshot existsDataSnapshot = await Task;
            bool exist = existsDataSnapshot.Exists;

            Debug.Log($"The {User.UserId} is a teacher? {exist} ");

            return exist;

        }


        /**
         * Function to update the Student's score upon successful completion of a stage
         * @param gamemode contains game mode, to only update the score of that mode
         * @param userid contains student's user id
         * @param justFinishedLevel contains level of the game student played
         * @param new_timeTaken latest time taken student took to complete the game
         * @param new_points lates number of points student earned during the game
         */
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
                    if (new_points < points[justFinishedlevel - 1])
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

        /**
         * Updates student's assignment score.
         * @param userid contains student's id
         * @param assignmentID contains the id of the assignment the score is assigned to
         * @param newPoints contains the points earned by student
         */
        async public static Task updateAssignmentScoreAsync(string userid, string assignmentID, int newPoints)
        {

            AssignmentResults DBResult = new AssignmentResults();
            var resultTask = DBreference.Child("AssignmentScore").Child(assignmentID).Child(userid).GetValueAsync();
            DataSnapshot playerScore = await resultTask;
            Debug.Log("get assignment score");
            string DBPlayerScorejsonString = playerScore.GetRawJsonValue();
            // UnityEngine.Debug.Log(DBPlayerScorejsonString);
            if (DBPlayerScorejsonString == null)
            {
                DBResult.attempts = 1;
                DBResult.points.Add(newPoints);
                DBResult.maxPoint = newPoints;

            }
            else
            {
                DBResult = JsonConvert.DeserializeObject<AssignmentResults>(DBPlayerScorejsonString);

                int attempts = DBResult.attempts;
                List<int> points = DBResult.points;

                DBResult.attempts++;
                DBResult.points.Add(newPoints);

                if (DBResult.points.Count == 0)
                    DBResult.maxPoint = newPoints;
                else
                {
                    int count = 0;
                    foreach (float point in DBResult.points)
                    {
                        if (newPoints >= point)
                            count++;
                    }
                    if (count == DBResult.points.Count)
                        DBResult.maxPoint = newPoints;
                }
            }

            string updatedUserScore = JsonConvert.SerializeObject(DBResult);
            var Task = DBreference.Child("AssignmentScore").Child(assignmentID).Child(userid).SetRawJsonValueAsync(updatedUserScore);
            await Task;
            Debug.Log("updated assignment score");


        }

        /**
         * Function Gets the Users' Max Level(maximum uncleared stage) from the database
         * @param gamemode contains game mode to check user's max level for that specific game mode
         */
        async public static Task<int> getUserMaxLevelReachedAsync(string gamemode)
        {
            UnityEngine.Debug.Log("reached");
            string uid = PhotonNetwork.player.UserId;
            var snapshotTask = DBreference.Child("Student").Child(uid).Child(gamemode).Child("curSubstage").GetValueAsync();
            DataSnapshot snapshot = await snapshotTask;
            int maxLevelReached = JsonConvert.DeserializeObject<int>(snapshot.GetRawJsonValue()); ;
            Debug.Log($"The maxLevelReached that was returned from database for {gamemode} is {maxLevelReached}");
            return maxLevelReached;
        }

        /**
         * This function gets the Top 10 users from the Leaderboard according to the gameMode specified
         * @param gamemode contains game mode to get top 10 users for that specific game mode
         */
        async public static Task<List<InitUser>> GetLeaderBoardFromDatabase(string gameMode)
        {
            List<InitUser> LeaderBoardUsers = new List<InitUser>();
            Debug.Log("Loading LeaderBoardSnapShot Task in FirebaseManager");
            var LeaderBoardSnapShotTask = DBreference.Child("Student").OrderByChild($"{gameMode}/totalPoints").GetValueAsync();
            DataSnapshot LeaderBoardSnapShot = await LeaderBoardSnapShotTask;
            if (LeaderBoardSnapShotTask.IsFaulted)
            {
                Debug.Log("LeaderBoardSnapShot Task in FirebaseManager fked up");
            }
            else
            {
                // Loop through every users UID
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


        /**
         * This function gets a List of Student's Names from the Database.
         * Can be deprecated and use CClasses CChild to get All uid Then Get all the Names.
         * @param className contains class name
         */
        async public static Task<Dictionary<string, string>> LoadStudentNamesAsync(string className)
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

        /**
         * This function creates an assignment for a Teacher
         */
        public static string getAssignmentKey()
        {
            string AssignmentID = DBreference.Child("Assignments").Push().Key;
            Debug.Log("Assignment Id is: " + AssignmentID);
            return AssignmentID;
        }

        /**
         * Function for teacher to assign an assignment to all students that have subscribed to the class.
         * @param className contains the class name that the teacher created the assignment for
         * @param AssignmentID contains the id of the assignment the teacher created
         */
        public async static Task addToAllSubscribedStudents(string className, string AssignmentID)
        {
            List<string> UsersInClass;
            //get all classes here
            var classTask = DBreference.Child("Classes").Child(className).GetValueAsync();
            DataSnapshot classSnapshot = await classTask;
            string inputjson = classSnapshot.GetRawJsonValue();
            Debug.Log($"The students in the class json is {inputjson}");

            UsersInClass = JsonConvert.DeserializeObject<List<string>>(inputjson);

            foreach (string uid in UsersInClass)
            {
                var task = GetUser(uid, "Student");
                InitUser user = await task;
                if (!user.assignments.Contains(AssignmentID))
                {
                    Debug.Log($"Student with {uid} successfully assigned assignment with {AssignmentID}");
                    //if there are no assignments in student's assignment
                    if (user.assignments.Contains("NONE"))
                    {
                        user.assignments[0] = AssignmentID;
                    }
                    else
                    {
                        user.assignments.Add(AssignmentID);
                    }

                    string updated_user = JsonConvert.SerializeObject(user.assignments);
                    var updatetask = DBreference.Child("Student").Child(uid).Child("assignments").SetRawJsonValueAsync(updated_user);
                    await updatetask;
                }
                else
                {
                    Debug.Log("Assignment already in student's assignment List");
                }
            }
        }
        async public static Task createAssignmentAsync(string AssignmentID)
        {
            DBreference.Child("Assignments").Child(AssignmentID);
            await DBreference.Child("Assignments").Child(AssignmentID).SetValueAsync(1);
            Debug.Log($"Successfully set Assignment with Assignment ID {AssignmentID}");
            //Add code to questions database 
        }

        /**
         * Helper function to check if username exist in database for registration
         * @param _username contains the username to be checked
         */
        async public static Task<bool> CheckUsernameExistsInDatabaseAsync(string _username)
        {
            var usernameTask = DBreference.Child("Usernames").GetValueAsync();
            DataSnapshot username = await usernameTask;
            bool hasUsername = username.Child(_username).Exists;
            Debug.Log($"Does username exist in database? {hasUsername}");
            return hasUsername;
        }

        /**
         * This method adds an intialised user data (Student or Teacher )to firebase database
         * @param acctype contains the type of the account to be created
         * @param username contains username to be added
         * @param uid contains user id
         */
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


        /**
         * Adds Uid of newly registered student to a class
         * @param className contains name of class that the student is registered for
         * @param uid contains id of user registering
         */
        private async static Task subscribeClass(string className, string uid)
        {
            List<string> ListOfSubscribers;
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

        /**
         * Function to load questions
         * @param stage contains game stage
         * @param substage contains substage of the stage
         * @param questionNo contains question number to be loaded
         */
        public async static Task<DBQT> getQuestionFromNormalDB(string stage, string substage, string questionNo)
        {
            DBQT singleQuestion;
            var Task = DBreference.Child("Questions").Child(stage).Child(substage).Child(questionNo).GetValueAsync();
            DataSnapshot singleQuestionSnapshot = await Task;
            string sqstr = singleQuestionSnapshot.GetRawJsonValue();
            singleQuestion = JsonConvert.DeserializeObject<DBQT>(sqstr);
            return singleQuestion;
        }

        /**
         * Function to get question from Custom Lobby database
         * @param roomName contains the room where questions are to be fetched from
         * @param quizNo contains quiz number
         * @param qnNo contains question number
         */
        public async static Task<DBQT> getQuestionFromCustomDB(string roomName, string quizNo, string qnNo)
        {
            DBQT singleQuestion;
            var Task = DBreference.Child("CustomLobbyQuestions").Child(roomName).Child(quizNo).GetValueAsync();
            DataSnapshot singleQuestionSnapshot = await Task;
            string sqstr = singleQuestionSnapshot.GetRawJsonValue();
            singleQuestion = JsonConvert.DeserializeObject<DBQT>(sqstr);
            return singleQuestion;
        }

        /**
         * Function to get question from Assignment database
         * @param roomName contains the room where questions are to be fetched from
         * @param quizNo contains quiz number
         * @param qnNo contains question number
         */
        public async static Task<DBQT> getQuestionFromAssignmentDB(string roomName, string quizNo, string qnNo)
        {
            Debug.Log("roomName: " + roomName + " quizNo: " + quizNo + " " + qnNo);
            DBQT singleQuestion;
            var Task = DBreference.Child("Assignments").Child(roomName).Child(quizNo).Child(qnNo).GetValueAsync();
            DataSnapshot singleQuestionSnapshot = await Task;
            string sqstr = singleQuestionSnapshot.GetRawJsonValue();
            singleQuestion = JsonConvert.DeserializeObject<DBQT>(sqstr);
            //Debug.Log("fbmanager assignments");
            return singleQuestion;
        }

        /**
         * Function to get assignment name
         * @param uid contains id of user
         */
        public async static Task<List<String>> getAssignmentName(string uid)
        {
            var Task = DBreference.Child("Student").Child(uid).Child("assignments").GetValueAsync();
            DataSnapshot assignment = await Task;
            string sqtr = assignment.GetRawJsonValue();
            List<string> assignmentList = JsonConvert.DeserializeObject<List<string>>(sqtr);
            List<string> returnList = new List<string>();
            foreach (string i in assignmentList)
            {

                //Debug.Log(i);
                returnList.Add(i);
            }
            return returnList;
        }

        /**
         * Function to get list of all assignments
         */
        public async static Task<List<String>> getAllAssignmentName()
        {
            List<string> assignmentList = new List<String>();
            var Task = DBreference.Child("Assignments").GetValueAsync();
            DataSnapshot assignment = await Task;

            Debug.Log($"There are {assignment.ChildrenCount} and {assignment.Reference.ToString()}");
            foreach (DataSnapshot childSnapShot in assignment.Children)
            {
                Debug.Log($"There are {assignment.ChildrenCount} and {childSnapShot.Reference.ToString()}");
                //string snapURL = childSnapShot.Reference.ToString();
                //string oneAssignment = snapURL.Substring(64);
                string oneAssignment = childSnapShot.Key;
                Debug.Log("one assignment = " + oneAssignment);
                //string childJsonString = childSnapShot.GetRawJsonValue();
                //string oneAssignment = JsonConvert.DeserializeObject<string>(childJsonString);

                assignmentList.Add(oneAssignment);
            }

            return assignmentList;
        }

        /**
         * Function to get list of students that has been assigned a specific assignment
         * @param assignmentID contains id of assignment to get student list from
         */
        public async static Task<List<String>> getAllStudentFromAssignments(string assignmentID)
        {
            List<string> studentList = new List<String>();
            var Task = DBreference.Child("AssignmentScore").Child(assignmentID).GetValueAsync();
            DataSnapshot students = await Task;

            foreach (DataSnapshot childSnapShot in students.Children)
            {
                Debug.Log($"There are {students.ChildrenCount} and {childSnapShot.Reference.ToString()}");
                //string snapURL = childSnapShot.Reference.ToString();
                //string oneStudent = snapURL.Substring(89);

                string studentResult = childSnapShot.Key;
                Debug.Log(studentResult);
                //Debug.Log("one assignment = " + oneAssignment);
                //string childJsonString = childSnapShot.GetRawJsonValue();
                //string oneAssignment = JsonConvert.DeserializeObject<string>(childJsonString);

                studentList.Add(studentResult);
            }
            return studentList;
        }

        /**
         * Function to get assignment results of a student for an assignment.
         * @param assignmentID contains id of assignment
         * @param studentName contains name of student to get results from
         */
        public async static Task<AssignmentResults> getAssignmentResults(string assignmentID, string studentName)
        {
            AssignmentResults results;
            Debug.Log("assignmentID= " + assignmentID + " studentname= " + studentName);
            var Task = DBreference.Child("AssignmentScore").Child(assignmentID).Child(studentName).GetValueAsync();
            DataSnapshot singleQuestionSnapshot = await Task;
            string sqstr = singleQuestionSnapshot.GetRawJsonValue();
            results = JsonConvert.DeserializeObject<AssignmentResults>(sqstr);
            //Debug.Log("fbmanager assignments");
            return results;
        }
        //public static InitUser GetUserrest(string uid,string accType)
        //{
        //    Debug.Log("reached user");
        //    InitUser resUser;
        //    return resUser= RestClient.Get<InitUser>(DatabaseEndPoint + $"Student/{uid}").Then<InitUser>(user=> 
        //    {
        //        Debug.Log("Raeached inside func");
        //    });
        //}


    }

}
