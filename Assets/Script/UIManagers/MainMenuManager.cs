using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string versionName = "0.2";
    public int mode = 0; //mode 0 = singleplayer, 1 = multiplayer, 2 = custom, 3 = assignment
    // Start is called before the first frame update
    public GameObject leaderboard, main;
    public GameObject backbtn;

    private void Awake()
    {

        Debug.Log("Main Menu Manager instantiated");
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
        PhotonNetwork.LoadLevel("CustomLobby");
    }
    /*public void mainUI()
    {
        clearscreen();
        main.SetActive(true);
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
        leaderboard.SetActive(false);
        main.SetActive(false);
    }*/

    

}
