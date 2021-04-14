using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using FullSerializer;
using Proyecto26;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Assets
{
    /**
    *  LoadQuestions loads the questions from Firebase for all game modes.
    */
    public class LoadQuestions : MonoBehaviour
    {
        // variables to input in scene
        private int StageNumber;
        private int SubstageNumber; // SubstageNumber = currentLevel%3

        public AbstractQuizManager MCQ1;
        public AbstractQuizManager MCQ2;
        public AbstractQuizManager MCQ3;
        public AbstractQuizManager SAQ1;
        public AbstractQuizManager SAQ2;

        public static string Question;
        public static string Answer;
        public static string Options;

        public QuestionAndAnswer QA;

        public QuestionAndAnswer[] QandAList = new QuestionAndAnswer[15]; // save all 15 questions for one substage
        public List<QuestionAndAnswer> MCQList = new List<QuestionAndAnswer>();
        public List<QuestionAndAnswer> SAQList = new List<QuestionAndAnswer>();
        /* old database
        private string databaseURL = "https://semaindb-default-rtdb.firebaseio.com/Questions/";
        private string AuthKey = "AIzaSyBqMbMl_ZIV17atZXrssFCnJERYpoffu8s";
        private string teacherPassword = "adminSE";
        private string teacherEmail = "admin@SE.com";
        */

        //private string databaseURL = "https://fir-auth-9c8cd-default-rtdb.firebaseio.com/Questions/";
        //private string customDBURL = "https://fir-auth-9c8cd-default-rtdb.firebaseio.com/CustomLobbyQuestions/";
        //private string AuthKey = "AIzaSyCp3-tVb1biSiZ4fASGQ_gUit-IZhko5mM";
        //private string teacherPassword = "password123";
        //private string teacherEmail = "teacher8@gmail.com";

        //public static fsSerializer serializer = new fsSerializer();

        //private string idToken;
        public static string localId;
        /*
        private string qnNoDBURL = "https://semaindb-default-rtdb.firebaseio.com/MCQuestionNo/";
        private string qn1URL = "https://semaindb-default-rtdb.firebaseio.com/Questions/Stage_1/Substage_1/4/";
        */
        private GameObject modeObject;
        private int mode;
        private int currentlevel;

        // Start is called before the first frame update 
        private async void Start()
        {
            modeObject = GameObject.Find("modeObject");
            mode = modeObject.GetComponent<mode>().modeType;
            Debug.Log(PlayerPrefs.GetInt("currentLevel", 1));
            currentlevel = PlayerPrefs.GetInt("currentLevel", 1);

            if (currentlevel % 3 != 0)
            {
                StageNumber = currentlevel / 3 + 1;
            }
            else
            {
                StageNumber = currentlevel / 3;
            }
            Debug.Log("Stage Number: " + StageNumber);

            SubstageNumber = currentlevel % 3;
            if (SubstageNumber == 0)
            {
                SubstageNumber = 3;
            }
            Debug.Log("Substage Number: " + SubstageNumber);

            //string userData = "{\"email\":\"" + teacherEmail + "\",\"password\":\"" + teacherPassword + "\",\"returnSecureToken\":true}";
            //RestClient.Post<SignResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=" + AuthKey, userData).Then(
            //    response =>
            //    {
            //        idToken = response.idToken;
            //        localId = response.localId;
            //        Debug.Log("start response done");
            //    }).Catch(error =>
            //    {
            //        Debug.Log(error);
            //    });
            //StartCoroutine(LateStart(0.5f));
            localId = PhotonNetwork.player.UserId;
            var task = LateStart();
            await task;


        }
        async Task LateStart()
        {
            //yield return new WaitForSeconds(waitTime);
            ////Your Function You Want to Call
            //Debug.Log("late start");
            var task = getAllQuestions();
            await task;

            assignQuestions();
        }
        void assignQuestions()
        {
            Debug.Log("start assign");
            //assign questions from lists to each abstractquizmanager QnA list
            MCQ1.QnA = MCQList.GetRange(0, 3);
            MCQ2.QnA = MCQList.GetRange(3, 3);
            MCQ3.QnA = MCQList.GetRange(6, 3);
            SAQ1.QnA = SAQList.GetRange(0, 3);
            SAQ2.QnA = SAQList.GetRange(3, 3);
            Debug.Log("questions assigned");
        }

        private async Task GetQuestionsFromNormalDB(int questionNumber, int StageNumber, int SubstageNumber)
        {
            //RestClient.Get<DBQT>(databaseURL + "Stage_" + StageNumber.ToString() + "/" + "Substage_" + SubstageNumber.ToString() +
            //"/" + i.ToString() + "/" + ".json?auth=" + idToken).Then(response =>
            //{
            //    Debug.Log("inside " + i);
            //    questions = response;
            //    Debug.Log("question " + i.ToString() + questions.Question + " " + questions.Answer + " " + questions.Options + " ");
            //    QandAList[i - 1] = new QuestionAndAnswer(questions.Question, questions.Options.Split(';'), questions.Answer);
            //    Debug.Log("REACHED" + QandAList[i - 1].Questions); // checking if QAndAList values were added
            //    Debug.Log(QandAList[i - 1].Questions + " options length =" + QandAList[i - 1].Answers.Length);
            //    if (QandAList[i - 1].Answers.Length == 1) //change this when QuestionAndAnswer type changes
            //    {
            //        SAQList.Add(QandAList[i - 1]);
            //        Debug.Log("Added SAQ" + QandAList[i - 1].Questions + QandAList[i - 1].CorrectAnswer);
            //    }
            //    else
            //    {
            //        MCQList.Add(QandAList[i - 1]);
            //        Debug.Log("Added MCQ" + QandAList[i - 1].Questions);
            //    }
            //});

            var getTask = FirebaseManager.getQuestionFromNormalDB($"Stage_{StageNumber}", $"Substage_{SubstageNumber}", questionNumber.ToString());
            DBQT question = await getTask;
            QandAList[questionNumber - 1] = new QuestionAndAnswer(question.Question, question.Options.Split(';'), question.Answer);
            Debug.Log("REACHED" + QandAList[questionNumber - 1].Questions); // checking if QAndAList values were added
            Debug.Log(QandAList[questionNumber - 1].Questions + " options length =" + QandAList[questionNumber - 1].Answers.Length);
            if (QandAList[questionNumber - 1].Answers.Length == 1) //change this when QuestionAndAnswer type changes
            {
                SAQList.Add(QandAList[questionNumber - 1]);
                Debug.Log("Added SAQ" + QandAList[questionNumber - 1].Questions + QandAList[questionNumber - 1].CorrectAnswer);
            }
            else
            {
                MCQList.Add(QandAList[questionNumber - 1]);
                Debug.Log("Added MCQ" + QandAList[questionNumber - 1].Questions);
            }

        }
        private async Task GetQuestionsFromCustomDB(string roomName, int i, int j)
        {
            //Debug.Log("correct " + customDBURL + PhotonNetwork.room.Name + "/" + "quiz_" + i.ToString() +
            //    "/" + j.ToString() + "/" + localId + ".json?auth=" + idToken);

            //RestClient.Get<DBQT>(customDBURL + PhotonNetwork.room.Name + "/" + "quiz_" + i.ToString() +
            //    "/" + j.ToString() + "/" + localId + ".json?auth=" + idToken).Then(response =>
            //{
            //    Debug.Log("inside " + i);
            //    questions = response;
            //    QandAList[i - 1] = new QuestionAndAnswer(questions.Question, questions.Options.Split(';'), questions.Answer);
            //    if (QandAList[i - 1].Answers.Length == 1) //change this when QuestionAndAnswer type changes
            //    {
            //        SAQList.Add(QandAList[i - 1]);
            //        Debug.Log("Added SAQ" + QandAList[i - 1].Questions);
            //    }
            //    else
            //    {
            //        MCQList.Add(QandAList[i - 1]);
            //        Debug.Log("Added MCQ" + QandAList[i - 1].Questions);
            //    }
            //});
            var getTask = FirebaseManager.getQuestionFromCustomDB(roomName, $"quiz_{i}", j.ToString());
            DBQT question = await getTask;
            QandAList[i - 1] = new QuestionAndAnswer(question.Question, question.Options.Split(';'), question.Answer);
            Debug.Log("REACHED" + QandAList[i - 1].Questions); // checking if QAndAList values were added
            Debug.Log(QandAList[i - 1].Questions + " options length =" + QandAList[i - 1].Answers.Length);
            if (QandAList[i - 1].Answers.Length == 1) //change this when QuestionAndAnswer type changes
            {
                SAQList.Add(QandAList[i - 1]);
                Debug.Log("Added SAQ" + QandAList[i - 1].Questions + QandAList[i - 1].CorrectAnswer);
            }
            else
            {
                MCQList.Add(QandAList[i - 1]);
                Debug.Log("Added MCQ" + QandAList[i - 1].Questions);
            }

        }

        private async Task GetAssignmentFromDB(string roomName, int i, int j)
        {
            var getTask = FirebaseManager.getQuestionFromAssignmentDB(roomName, $"quiz_{i}", j.ToString());
            DBQT question = await getTask;
            QandAList[i - 1] = new QuestionAndAnswer(question.Question, question.Options.Split(';'), question.Answer);
            Debug.Log("REACHED" + QandAList[i - 1].Questions); // checking if QAndAList values were added
            Debug.Log(QandAList[i - 1].Questions + " options length =" + QandAList[i - 1].Answers.Length);
            if (QandAList[i - 1].Answers.Length == 1) //change this when QuestionAndAnswer type changes
            {
                SAQList.Add(QandAList[i - 1]);
                Debug.Log("Added SAQ" + QandAList[i - 1].Questions + QandAList[i - 1].CorrectAnswer);
            }
            else
            {
                MCQList.Add(QandAList[i - 1]);
                Debug.Log("Added MCQ" + QandAList[i - 1].Questions);
            }

        }
        public async Task getAllQuestions()
        {
            Debug.Log("button");
            Debug.Log("mode = " + mode);
            //load all questions from DB
            if (mode == 0 || mode == 1)
            {
                for (int i = 1; i < 16; i++)
                {
                    var task = GetQuestionsFromNormalDB(i, StageNumber, SubstageNumber);
                    await task;
                }
            }
            else if (mode == 2)
            {
                for (int i = 1; i < 6; i++)
                {
                    for (int j = 1; j < 4; j++)
                    {
                        var task = GetQuestionsFromCustomDB(PhotonNetwork.room.Name, i, j);
                        await task;
                    }
                }
            }
            else if (mode == 3)
            {
                Debug.Log("mode = 3");
                for (int i = 1; i < 6; i++)
                {
                    for (int j = 1; j < 4; j++)
                    {
                        var task = GetAssignmentFromDB(PhotonNetwork.room.Name, i, j);
                        await task;
                    }
                }
                Debug.Log("load all questions done");
            }

        }


    }
}