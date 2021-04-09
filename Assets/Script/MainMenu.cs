using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string versionName = "0.2";
    public bool isMultiplayer = false;
    public bool isCustom = false;
    // Start is called before the first frame update
    public GameObject summary, leaderboard, main;
    public GameObject backbtn;
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        //connect to photon
        PhotonNetwork.ConnectUsingSettings(versionName);
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
    public void summaryReport()
    {
        summary.SetActive(true);
        main.SetActive(false);
        backbtn.SetActive(true);

    }

        //Leaderboard button
    //public void LeaderBoardButton()
    //{
    //    StartCoroutine(LoadLeaderBoardData("customPlayer", scoreboardCustomContent));
    //    StartCoroutine(LoadLeaderBoardData("multiPlayer", scoreboardMultiContent));
    //    StartCoroutine(LoadLeaderBoardData("singlePlayer", scoreboardSingleContent));
    //}

    public void leaderBoard()
    {
        leaderboard.SetActive(true);
        main.SetActive(false);
        backbtn.SetActive(true);
    }
    public void back()
    {
        summary.SetActive(false);
        leaderboard.SetActive(false);
        main.SetActive(true);
    }
    

}
