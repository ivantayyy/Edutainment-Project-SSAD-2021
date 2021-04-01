using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using FullSerializer;
using Proyecto26;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public class CustomQuestion : MonoBehaviour
{
    private string databaseURL = "https://fir-auth-9c8cd-default-rtdb.firebaseio.com/CustomLobbyQuestions/";
    private string AuthKey = "AIzaSyCp3-tVb1biSiZ4fASGQ_gUit-IZhko5mM";
    private string userPassword = "password123";
    private string userEmail = "teacher8@gmail.com";

    public static fsSerializer serializer = new fsSerializer();

    private string idToken;
    public static string localId;

    public InputField getLobbyName;
    public InputField getQuestion;
    public InputField getOptions;
    public InputField getAnswer;
    public Dropdown questionTypeSelection;
    public Text quizCounterDisplay;
    public Text questionCounterDisplay;
    public Text warningDisplay;
    public Dropdown selectQuestion;
    public Dropdown selectQuiz;
    public GameObject continueButton;

    public int quizCounters = 1;
    public int questionCounters = 1;
    private int questionTypeCounter = 0;
    private string questionType = null;
    private string newQuestionType = null;
    private int setType = -1;
    private int checkType = -1;
    private int quizNo = -1;
    private int questionNo = -1;
    int quizCounterHolder = -1;
    int questionCounterHolder = -1;
    bool allQuestionsCreated=false;


    MCQData questionData = new MCQData();
    /*private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings("0.2");
    }

    private void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected");
    }*/
    private void Start()
    { 
        string userData = "{\"email\":\"" + userEmail + "\",\"password\":\"" + userPassword + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>("https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=" + AuthKey, userData).Then(
            response =>
            {
                idToken = response.idToken;
                localId = response.localId;
            }).Catch(error =>
            {
                Debug.Log(error);
            });
        Debug.Log("test2"); 
    }
    private void Update()
    {
        //if 1 question created, continue button set active (for testing only)
        /*if (quizCounters == 1 && questionCounters == 2)
            allQuestionsCreated = true;*/
        if (allQuestionsCreated)
            continueButton.SetActive(true);
    }
    public void nextScene()
    {
        PhotonNetwork.CreateRoom(getLobbyName.text, new RoomOptions() { maxPlayers = 2 }, null);
        PhotonNetwork.LoadLevel("Lobby");
    }
    public void OnSubmit()
    {
        PostToDatabase();
        questionTypeChecker();
        getAnswer.text = "";
        getQuestion.text = "";
        getOptions.text = "";

    }

    public void submitQuestionChange()
    {
        retrieveCustomInfo();
    }

    public void backButton()
    {
        PhotonNetwork.LoadLevel("Main Menu");
    }
    private void PostToDatabase()
    {
       
        User user = new User();
        MCQData mcqData = new MCQData();

        mcqData.Answer = getAnswer.text;
        mcqData.Question = getQuestion.text;
        mcqData.Options = getOptions.text;

        Debug.Log(quizCounters + " " + questionCounters);
        

        questionCounterHolder = questionCounters + 1;
        quizCounterHolder = quizCounters;
        if (questionCounters == 3 && quizCounters == 5)
        {
            quizCounterHolder = 1;
            questionCounterHolder = 1;
            quizCounterDisplay.text = "Quiz : " + quizCounterHolder.ToString() + " / 5 Quizzes";
            questionCounterDisplay.text = "Question: " + questionCounterHolder.ToString() + " / 3 Questions";
        }
        else if (questionCounters == 3)
        {
            questionCounterHolder = 1;
            quizCounterHolder = quizCounters + 1;
            quizCounterDisplay.text = "Quiz : " + quizCounterHolder.ToString() + " / 5 Quizzes";
            questionCounterDisplay.text = "Question: " + questionCounterHolder.ToString() + " / 3 Questions";
        }
        else
        {
            quizCounterDisplay.text = "Quiz : " + quizCounterHolder.ToString() + " / 5 Quizzes";
            questionCounterDisplay.text = "Question: " + questionCounterHolder.ToString() + " / 3 Questions";
        }
        

        RestClient.Put(databaseURL + getLobbyName.text + "/" + "quiz_" + quizCounters.ToString() + "/" + questionCounters.ToString() + "/" + localId + ".json?auth=" + idToken, mcqData);

        if (questionCounters == 3 )
        {
            quizCounters = quizCounters + 1;
        }
        questionCounters = questionCounters + 1;

        if (quizCounters == 6 && questionCounters == 4)
        {
            Debug.Log("Reached 15 Questions");
            quizCounters = 1;
            questionCounters = 1;
            allQuestionsCreated = true;
        }
        else if (questionCounters == 4 && quizCounters <= 5)
        {
            
            questionCounters = 1;
            
        }
    }

    private void questionTypeChecker()
    {

        if (questionTypeCounter > 2)
        {
            questionTypeCounter = 0;
        }
        if (questionTypeCounter == 0)
        {
            questionType = getOptions.text;
            
            if (String.IsNullOrEmpty(questionType))
            {
                setType = 0;
            }
            else
            {
                setType = 1;
            }
            
        }
        newQuestionType = getOptions.text;
        if (String.IsNullOrEmpty(newQuestionType) && questionTypeCounter!=0)
        {
            checkType = 0;
        }
        else
        {
            checkType = 1;
        }

        
        if (setType != checkType && questionTypeCounter != 0)
        {
            Debug.Log("Error question type not the same");
            warningDisplay.text = "Error for previous question, please go back and re enter question.";
        }
        else
        {
            warningDisplay.text = "";
        }
        questionTypeCounter += 1;
        
    }

    private void retrieveCustomInfo()
    {
        Debug.Log("tsad");
        questionNo = selectQuestion.value + 1;
        quizNo = selectQuiz.value + 1;

        RestClient.Get<MCQData>(databaseURL + getLobbyName.text + "/" + "quiz_" + quizNo.ToString() + "/" + questionNo.ToString() + "/" + localId + ".json?auth=" + idToken).Then(response =>
        {
            Debug.Log("hello");
            questionData = response;
            reInsertQuestion();

        });

        
    }    
    private void reInsertQuestion()
    {
        int quizSelect = selectQuiz.value + 1;
        int questionSelect = selectQuestion.value + 1;

        quizCounters = quizSelect;
        questionCounters = questionSelect;

        Debug.Log(questionData.Options);
        getQuestion.text = questionData.Question;
        getAnswer.text = questionData.Answer;
        getOptions.text = questionData.Options;

        Debug.Log(quizCounters + " " + questionCounters);

    }
}
