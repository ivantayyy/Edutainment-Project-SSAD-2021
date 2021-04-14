using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeacherMenuUIManager : MonoBehaviour
{
    public static TeacherMenuUIManager instance;

    [Header("Teacher Main Menu UIs")]
    public bool isTeacher = false;
    public GameObject SummaryReportUI;
    public GameObject StudentSummaryReportUI;
    public GameObject TeacherMenuUI;
    void Start()
    {
        
        if(instance == null)
        {
            instance = this;
            clearscreen();
            TeacherMenuUI.SetActive(true);
            DontDestroyOnLoad(transform.gameObject);
            isTeacher = true;
        }
    }

    // Update is called once per frame
    public void summaryReport()
    {
        clearscreen();
        SummaryReportUI.SetActive(true);
    }
    public void back()
    {
        clearscreen();
        TeacherMenuUI.SetActive(true);
    }
    public void clearscreen()
    {
        SummaryReportUI.SetActive(false);
        StudentSummaryReportUI.SetActive(false);
        TeacherMenuUI.SetActive(false);
    }
    public void studentSummaryReportScreen() //scoreboard button
    {
        clearscreen();
        StudentSummaryReportUI.SetActive(true);
    }
    public void createAssignment()
    {
        SceneManager.LoadScene("CustomLobbyCreation");
    }
    public void assignmentResults()
    {
        SceneManager.LoadScene("AssignmentResults");
    }
}
