using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    public static MainMenuUIManager instance;
    public GameObject mainMenuUI;
    public GameObject leaderBoardUI;
    public GameObject backbtn;
    private void Awake()
    {
        if (instance == null)
        {  
            mainUI();
            instance = this;
        }

    }

    public void mainUI()
    {
        clearscreen();
        mainMenuUI.SetActive(true);
        backbtn.SetActive(true);
    }

    public void leaderBoard()
    {
        clearscreen();
        leaderBoardUI.SetActive(true);
    }
    public void back()
    {
        clearscreen();
        mainMenuUI.SetActive(true);
    }
    public void clearscreen()
    {
        leaderBoardUI.SetActive(false);
        mainMenuUI.SetActive(false);
    }
}
