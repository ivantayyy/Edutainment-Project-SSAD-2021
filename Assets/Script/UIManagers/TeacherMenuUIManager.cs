using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets
{
    public class TeacherMenuUIManager : MonoBehaviour
    {
        public static TeacherMenuUIManager instance;

        [Header("Teacher Main Menu UIs")]
        public GameObject SummaryReportUI;
        public GameObject StudentSummaryReportUI;
        public GameObject TeacherMenuUI;
        public isTeacherObject isTeacherObj;
        void Start()
        {

            if (instance == null)
            {
                instance = this;
                clearscreen();
                TeacherMenuUI.SetActive(true);
                isTeacherObj.isTeacher = true;
            }
        }

        /**
         * Displays summary report UI.
         */
        public void summaryReport()
        {
            clearscreen();
            SummaryReportUI.SetActive(true);
        }

        /**
         * Returns to teacher menu UI.
         */
        public void back()
        {
            clearscreen();
            TeacherMenuUI.SetActive(true);
        }

        /**
         * Clears all screens
         */
        public void clearscreen()
        {
            SummaryReportUI.SetActive(false);
            StudentSummaryReportUI.SetActive(false);
            TeacherMenuUI.SetActive(false);
        }

        /**
         * Displays student summary report UI
         */
        public void studentSummaryReportScreen() //scoreboard button
        {
            clearscreen();
            StudentSummaryReportUI.SetActive(true);
        }

        /**
         * Displays custom lobby question creation UI.
         */
        public void createAssignment()
        {
            SceneManager.LoadScene("CustomLobbyQuestionCreation");
        }

        /**
         * Displays assignment results UI.
         */
        public void assignmentResults()
        {
            SceneManager.LoadScene("AssignmentResults");
        }
    }
}