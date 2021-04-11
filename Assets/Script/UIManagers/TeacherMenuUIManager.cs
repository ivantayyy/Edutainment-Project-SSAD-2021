using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherMenuUIManager : MonoBehaviour
{
    public static TeacherMenuUIManager instance;

    [Header("Teacher Main Menu UIs")]
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
}
