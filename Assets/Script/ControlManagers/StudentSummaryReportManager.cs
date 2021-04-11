using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StudentSummaryReportManager : MonoBehaviour
{
    public static StudentSummaryReportManager instance;

    [Header("StudentSummaryReport")]
    public Text UsernameText;
    public Text ClassSubscribedText;
    public Text useridText;
    
    public Transform singlePlayerContent;
    public Transform multiPlayerContent;
    public Transform customPlayerContent;
    
    public GameObject stageEntryElement;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            
            instance = this;
            Debug.Log("StudentSummaryReportManager instantiated");
        }

    }

    async public void loadStudentInfo(string uid)
    {
        UnityEngine.Debug.Log("Reached LoadStudent Function");
        var studentTask = FirebaseManager.GetUser(uid, "Student");
        InitUser student = await studentTask;

        UnityEngine.Debug.Log("Reached LoadStudent Function After getting user");
        LoadAllStudentData(student);

        UnityEngine.Debug.Log("LoadSinglePlayerData() successfully completed");

    }

    //Loads all the student data for all 3 boards
    private void LoadAllStudentData(InitUser student)
    {
        string username = student.username;
        UsernameText.text = username;
        useridText.text = student.id;
        ClassSubscribedText.text = student.classSubscribed;

        LoadSelectGameModeData(student, "singlePlayer");
        LoadSelectGameModeData(student, "multiPlayer");
        LoadSelectGameModeData(student, "customPlayer");
    }

    //Helper function for loading a single gamemode's data
    private void LoadSelectGameModeData(InitUser student, string GameMode)
    {
        
        Scores score;
        Transform content;
        if (GameMode == "singlePlayer")
        {
            score = student.singlePlayer;
            content = singlePlayerContent.transform;

        }else if(GameMode == "multiPlayer")
        {
            score = student.multiPlayer;
            content = multiPlayerContent.transform;

        }
        else
        {
            score = student.customPlayer;
            content = customPlayerContent.transform;

        }


        int CurSubstage = score.curSubstage;
        List<int> Attempts = score.attempts;
        List<float> TimeTaken = score.timeTaken;
        List<float> points = score.points;
        float totalPoints = score.totalPoints;
        //Load Data on screen

        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
            UnityEngine.Debug.Log("Destroyed Child");
        }

        if (CurSubstage == 1)
        {
            GameObject scoreBoardElement = Instantiate(stageEntryElement, content);
            scoreBoardElement.GetComponent<StageEntry>().NewStageEntry("Stage No", "Attempt No", "Fastest Time Taken", "Max Points");
            scoreBoardElement = Instantiate(stageEntryElement, content);
            scoreBoardElement.GetComponent<StageEntry>().NewStageEntry("NONE", "NONE", "NONE", "NONE");
        }
        else
        {
            GameObject scoreBoardElement = Instantiate(stageEntryElement, content);
            scoreBoardElement.GetComponent<StageEntry>().NewStageEntry("Stage No", "Attempt No", "Fastest Time Taken", "Max Points");

            for (int i = 0; i < CurSubstage - 1; i++)
            {
                int StageInt = i + 1;
                string StageString = $"Stage {StageInt.ToString()}";
                string NoOfAttempts = $"{Attempts.ElementAt(i).ToString()}";
                string TimeTakenString = $"{TimeTaken.ElementAt(i).ToString()}";
                string pointsOnSubstage = $"{points.ElementAt(i).ToString()}";

                Debug.Log($"on stage iteration {i.ToString()}: \nstage is {StageString} \n noOfAttempts is {NoOfAttempts} \n TimeTaken is {TimeTakenString} \n pointsOnSubstage is {pointsOnSubstage}");

                scoreBoardElement = Instantiate(stageEntryElement, content);
                scoreBoardElement.GetComponent<StageEntry>().NewStageEntry(StageString, NoOfAttempts, TimeTakenString, pointsOnSubstage);
            }
        }
    }

}
