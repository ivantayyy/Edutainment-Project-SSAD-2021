﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string versionName = "0.2";
    public bool isMultiplayer = false;
    public bool isCustom = false;
    // Start is called before the first frame update
    public GameObject summary, leaderboard, main,studentSummaryReport;
    public GameObject backbtn;

    public static MainMenu instance;
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(transform.gameObject);
            //connect to photon
            PhotonNetwork.ConnectUsingSettings(versionName);
            mainUI();
            instance = this;
        }
        
    }
    private void OnConnectedToMaster()//when connected to photon netwoek
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);//define lobby tyoe of photon
        Debug.Log("Connected");
    }
    public void singlePlayer()//link to single player button
    {
        //when select sinngleplayer, go to singple player page
        isMultiplayer = false;
        isCustom = false;
        //LOAD LEVEL IN PHOTON
        PhotonNetwork.LoadLevel("ChooseCharacters");
    }
    public void multiPlayer()
    {

        isMultiplayer = true;
        isCustom = false;
        Debug.Log("mm multi = " + isMultiplayer);
        PhotonNetwork.LoadLevel("Multiplayer");
    }
    public void custom()
    {
        isCustom = true;
        isMultiplayer = false;
        PhotonNetwork.LoadLevel("CustomLobbyCreation");
    }
    public void mainUI()
    {
        clearscreen();
        main.SetActive(true);
    }
    public void summaryReport()
    {
        clearscreen();
        summary.SetActive(true);
        backbtn.SetActive(true);
    }

    public void leaderBoard()
    {
        clearscreen();
        leaderboard.SetActive(true);
    }
    public void back()
    {
        clearscreen();
        main.SetActive(true);
    }
    public void clearscreen()
    {
        summary.SetActive(false);
        studentSummaryReport.SetActive(false);
        leaderboard.SetActive(false);
        main.SetActive(false);
    }

    public void studentSummaryReportScreen() //scoreboard button
    {
        clearscreen();
        studentSummaryReport.SetActive(true);
    }

}
