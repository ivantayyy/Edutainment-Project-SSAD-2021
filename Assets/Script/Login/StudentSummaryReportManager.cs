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
        LoadPlayerData(student);

        UnityEngine.Debug.Log("LoadSinglePlayerData() successfully completed");

    }

    public void LoadPlayerData(InitUser student)
    {
        string username = student.username;
        UsernameText.text = username;
        useridText.text = student.id;

        string classSubscribed=student.classSubscribed;
        ClassSubscribedText.text = classSubscribed;

        Scores multiPlayer = student.multiPlayer;
        Scores customPlayer = student.customPlayer;
        Scores singlePlayer = student.singlePlayer;
        
        int curSubstage = student.singlePlayer.curSubstage;
        List<int> singleAttempts = student.singlePlayer.attempts;
        List<float> singleTimeTaken = student.singlePlayer.timeTaken;
        List<float> singlepoints = student.singlePlayer.points;
        float singletotalPoints = student.singlePlayer.totalPoints;

        foreach (Transform child in singlePlayerContent.transform)
        {
            Destroy(child.gameObject);
            UnityEngine.Debug.Log("Destroyed Child");
        }


        //instantiate like this tring StageText,string Attempt, string TimeTakenText,string points
        for (int i = 0; i < curSubstage - 1; i++)
        {
            int StageInt = i + 1;
            string StageString = StageInt.ToString();
            string NoOfAttempts = singleAttempts.ElementAt(i).ToString();
            string TimeTakenString = singleTimeTaken.ElementAt(0).ToString();
            string pointsOnSubstage = singlepoints.ElementAt(0).ToString();

            Debug.Log($"on stage iteration {i.ToString()}: \nstage is {StageString} \n noOfAttempts is {NoOfAttempts} \n TimeTaken is {TimeTakenString} \n pointsOnSubstage is {pointsOnSubstage}");

            GameObject scoreBoardElement = Instantiate(stageEntryElement, singlePlayerContent);
            scoreBoardElement.GetComponent<StageEntry>().NewStageEntry(StageString, NoOfAttempts, TimeTakenString, pointsOnSubstage);
        }


        List<int> multiAttempts = student.multiPlayer.attempts;
        List<float> multiTimeTaken = student.multiPlayer.timeTaken;
        List<float> multipoints = student.multiPlayer.points;
        float multitotalPoints = student.multiPlayer.totalPoints;

        foreach (Transform child in multiPlayerContent.transform)
        {
            Destroy(child.gameObject);
            UnityEngine.Debug.Log("Destroyed Child");
        }


        //instantiate like this tring StageText,string Attempt, string TimeTakenText,string points
        for (int i = 0; i < curSubstage - 1; i++)
        {
            int StageInt = i + 1;
            string StageString = StageInt.ToString();
            string NoOfAttempts = multiAttempts.ElementAt(i).ToString();
            string TimeTakenString = multiTimeTaken.ElementAt(0).ToString();
            string pointsOnSubstage = multipoints.ElementAt(0).ToString();

            Debug.Log($"on stage iteration {i.ToString()}: \nstage is {StageString} \n noOfAttempts is {NoOfAttempts} \n TimeTaken is {TimeTakenString} \n pointsOnSubstage is {pointsOnSubstage}");

            GameObject scoreBoardElement = Instantiate(stageEntryElement, multiPlayerContent);
            scoreBoardElement.GetComponent<StageEntry>().NewStageEntry(StageString, NoOfAttempts, TimeTakenString, pointsOnSubstage);
        }

        List<int> customAttempts = student.customPlayer.attempts;
        List<float> customTimeTaken = student.customPlayer.timeTaken;
        List<float> custompoints = student.customPlayer.points;
        float customtotalPoints = student.customPlayer.totalPoints;
        
        foreach (Transform child in customPlayerContent.transform)
        {
            Destroy(child.gameObject);
            UnityEngine.Debug.Log("Destroyed Child");
        }
        for (int i = 0; i < curSubstage - 1; i++)
        {
            int StageInt = i + 1;
            string StageString = StageInt.ToString();
            string NoOfAttempts = customAttempts.ElementAt(i).ToString();
            string TimeTakenString = customTimeTaken.ElementAt(0).ToString();
            string pointsOnSubstage = custompoints.ElementAt(0).ToString();

            Debug.Log($"on stage iteration {i.ToString()}: \nstage is {StageString} \n noOfAttempts is {NoOfAttempts} \n TimeTaken is {TimeTakenString} \n pointsOnSubstage is {pointsOnSubstage}");

            GameObject scoreBoardElement = Instantiate(stageEntryElement, customPlayerContent);
            scoreBoardElement.GetComponent<StageEntry>().NewStageEntry(StageString, NoOfAttempts, TimeTakenString, pointsOnSubstage);
        }

        //foreach(int attempt in Attempts)
        //{
        //    Debug.Log($"Attempt {attempt}");
        //}
        //foreach (float timetaken in TimeTaken)
        //{
        //    Debug.Log($"TimeTaken {timetaken}");
        //}
        //foreach (float point in points)
        //{
        //    Debug.Log($"point {point}");
        //}
        //current max level


    }
}
