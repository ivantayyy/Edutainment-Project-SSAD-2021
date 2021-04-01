using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //Screen object variables
    public GameObject loginUI;
    public GameObject registerUI;
    public GameObject leaderBoardUI;
    public GameObject mainUI;
    public GameObject summaryReportUI;

    private void Awake()
    {
        //DEBUG
        if (instance == null)
        {
            instance = this;
            LoginScreen();            
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
            LoginScreen();

        }
    }

    //Functions to change the login screen UI
    public void LoginScreen() //Back button
    {
        clearscreen();
        loginUI.SetActive(true);
    }
    public void RegisterScreen() // Regester button
    {
        clearscreen();
        registerUI.SetActive(true);
    }

    public void clearscreen() //turn off all screens
    {
        summaryReportUI.SetActive(false);
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        leaderBoardUI.SetActive(false);
        mainUI.SetActive(false);
    }

    public void leaderBoardScreen() //logged in
    {
        clearscreen();
        leaderBoardUI.SetActive(true);
    }

    public void mainScreen() //logged in
    {
        clearscreen();
        mainUI.SetActive(true);
    }
    public void summaryReportScreen()
    {
        clearscreen();
        summaryReportUI.SetActive(true);
    }
    //public void scoreboardscreen() //scoreboard button
    //{
    //    clearscreen();
    //    scoreboardui.setactive(true);
    //}
}