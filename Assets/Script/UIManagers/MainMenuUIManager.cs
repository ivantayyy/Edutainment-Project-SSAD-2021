using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    public static MainMenuUIManager instance;

    //Screen object variables
    public GameObject mainMenuUI;
    public GameObject leaderBoardUI;

    private void Awake()
    {
        //DEBUG
        if (instance == null)
        {
            instance = this;
            mainUI();
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
            mainUI();

        }
    }

    //Functions to change the login screen UI
    public void leaderBoard()
    {
        clearscreen();
        leaderBoardUI.SetActive(true);
    }
    public void mainUI()
    {
        clearscreen();
        mainMenuUI.SetActive(true);
        //backbtn.SetActive(true);
    }

    public void clearscreen() //turn off all screens
    {
        mainMenuUI.SetActive(false);
        leaderBoardUI.SetActive(false);
    }

    public void backButton()
    {
        clearscreen();
        mainMenuUI.SetActive(true);
    }
}
