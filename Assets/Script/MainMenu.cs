using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string versionName = "0.2";
    public int mode = 0; //mode 0 = singleplayer, 1 = multiplayer, 2 = custom, 3 = assignment
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
        this.mode = 0;
        Debug.Log("mode = " + mode);
        //LOAD LEVEL IN PHOTON
        PhotonNetwork.LoadLevel("ChooseCharacters");
    }
    public void multiPlayer()
    {
        this.mode = 1;
        
        Debug.Log("mode = " + mode);
        PhotonNetwork.LoadLevel("Multiplayer");
    }
    public void custom()
    {
        this.mode = 2;
        Debug.Log("mode = " + mode);
        PhotonNetwork.LoadLevel("CustomLobbyCreation");
    }
    public void summaryReport()
    {
        summary.SetActive(true);
        main.SetActive(false);
        backbtn.SetActive(true);

    }
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
