using System.Collections;
using System.Collections.Generic;
using FullSerializer;
using Proyecto26;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LoadQuestions : MonoBehaviour
{
    // variables to input in scene
    public int StageNumber;
    public int SubstageNumber; // SubstageNumber = level%3

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

    private string databaseURL = "https://fir-auth-9c8cd-default-rtdb.firebaseio.com/Questions/";
    private string customDBURL = "https://fir-auth-9c8cd-default-rtdb.firebaseio.com/CustomLobbyQuestions/";
    private string AuthKey = "AIzaSyCp3-tVb1biSiZ4fASGQ_gUit-IZhko5mM";
    private string teacherPassword = "password123";
    private string teacherEmail = "teacher123@gmail.com";

    public static fsSerializer serializer = new fsSerializer();

    private string idToken;
    public static string localId;
    /*
    private string qnNoDBURL = "https://semaindb-default-rtdb.firebaseio.com/MCQuestionNo/";
    private string qn1URL = "https://semaindb-default-rtdb.firebaseio.com/Questions/Stage_1/Substage_1/4/";
    */
    DBQT questions = new DBQT();
    private GameObject mainMenuScript;
    private int mode;

    // Start is called before the first frame update 
    private void Start()
    {
        mainMenuScript = GameObject.Find("MainMenuScript");
        mode = mainMenuScript.GetComponent<MainMenu>().mode;
        string userData = "{\"email\":\"" + teacherEmail + "\",\"password\":\"" + teacherPassword + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=" + AuthKey, userData).Then(
            response =>
            {
                idToken = response.idToken;
                localId = response.localId;
                Debug.Log("start response done");
            }).Catch(error =>
            {
                Debug.Log(error);
            });
        StartCoroutine(LateStart(0.5f));
    }
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //Your Function You Want to Call
        Debug.Log("late start");
        getAllQuestions();
        StartCoroutine(assignQuestions(2f));
    }
    IEnumerator assignQuestions(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //assign questions from lists to each abstractquizmanager QnA list
        MCQ1.QnA = MCQList.GetRange(0, 3);
        MCQ2.QnA = MCQList.GetRange(3, 3);
        MCQ3.QnA = MCQList.GetRange(6, 3);
        SAQ1.QnA = SAQList.GetRange(0, 3);
        SAQ2.QnA = SAQList.GetRange(3, 3);
        Debug.Log("questions assigned");
    }

    private void GetQuestionsFromNormalDB(int i)
    {
        RestClient.Get<DBQT>(databaseURL + "Stage_" + StageNumber.ToString() + "/" + "Substage_" + SubstageNumber.ToString() +
        "/" + i.ToString() + "/" + ".json?auth=" + idToken).Then(response =>
        {
            Debug.Log("inside " + i);
            questions = response;
            //Debug.Log("question " + i.ToString() + questions.Question + " " + questions.Answer + " " + questions.Options + " ");
            QandAList[i - 1] = new QuestionAndAnswer(questions.Question, questions.Options.Split(';'), questions.Answer);
            //Debug.Log("REACHED" + QandAList[i - 1].Questions); // checking if QAndAList values were added
            //Debug.Log(QandAList[i - 1].Questions + " options length =" + QandAList[i - 1].Answers.Length);
            if (QandAList[i - 1].Answers.Length == 1) //change this when QuestionAndAnswer type changes
            {
                SAQList.Add(QandAList[i - 1]);
                Debug.Log("Added SAQ" + QandAList[i - 1].Questions+ QandAList[i - 1].CorrectAnswer);
            }
            else
            {
                MCQList.Add(QandAList[i - 1]);
                //Debug.Log("Added MCQ" + QandAList[i - 1].Questions);
            }
        });
    }
    private void GetQuestionsFromCustomDB(int i, int j)
    {
        Debug.Log("correct " + customDBURL + PhotonNetwork.room.Name + "/" + "quiz_" + i.ToString() +
            "/" + j.ToString() + "/" + localId + ".json?auth=" + idToken);
        
        RestClient.Get<DBQT>(customDBURL + PhotonNetwork.room.Name + "/" + "quiz_" + i.ToString() +
            "/" + j.ToString() + "/" + localId + ".json?auth=" + idToken).Then(response =>
        {
            Debug.Log("inside " + i);
            questions = response;
            QandAList[i - 1] = new QuestionAndAnswer(questions.Question, questions.Options.Split(';'), questions.Answer);
            if (QandAList[i - 1].Answers.Length == 1) //change this when QuestionAndAnswer type changes
            {
                SAQList.Add(QandAList[i - 1]);
                Debug.Log("Added SAQ" + QandAList[i - 1].Questions);
            }
            else
            {
                MCQList.Add(QandAList[i - 1]);
                Debug.Log("Added MCQ" + QandAList[i - 1].Questions);
            }
        });
    }
    public void getAllQuestions()
    {
        Debug.Log("button");
        Debug.Log("mode = " + mode);
        //load all questions from DB
        if (mode == 0 || mode ==1)
        {
            for (int i = 1; i < 16; i++)
            {
                GetQuestionsFromNormalDB(i);
            }
        }
        else if (mode == 2)
        {
            for(int i = 1;i<6;i++)
            {
                for(int j = 1;j<4;j++)
                {
                    GetQuestionsFromCustomDB(i, j);
                }
            }
        }
            
    }


}