using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class StudentSummaryReportManager : MonoBehaviour
{
    private InitUser student;
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
        }

    }

    async public void loadStudentInfo(string uid)
    {
        UnityEngine.Debug.Log("Reached LoadStudent Function");
        student = await FirebaseManager.GetUser(uid, "Student");
        UnityEngine.Debug.Log("Reached LoadStudent Function After getting user");
        LoadSinglePlayerData();
        UnityEngine.Debug.Log("LoadSinglePlayerData() successfully completed");

    }

    public void LoadSinglePlayerData()
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
        List<int> Attempts = student.singlePlayer.attempts;
        List<float> TimeTaken = student.singlePlayer.timeTaken;
        List<float> points = student.singlePlayer.points;
        float totalPoints = student.singlePlayer.totalPoints;

        foreach (Transform child in singlePlayerContent.transform)
        {
            UnityEngine.Debug.Log("reached before transform loop");
            Destroy(child.gameObject);
            UnityEngine.Debug.Log("reached after transform loop");
        }


        //instantiate like this tring StageText,string Attempt, string TimeTakenText,string points
        for(int i= 0; i<=curSubstage-2; i++ )
        {
            int StageInt = i + 1;
            string StageString = i+StageInt.ToString();

            string NoOfAttempts = Attempts[i].ToString();
            string TimeTakenString = TimeTaken[i].ToString();
            string pointsOnSubstage = points[i].ToString();

            GameObject scoreBoardElement = Instantiate(stageEntryElement, singlePlayerContent);
            scoreBoardElement.GetComponent<StageEntry>().NewStageEntry(StageString, NoOfAttempts, TimeTakenString, pointsOnSubstage);
        }
    }
}
