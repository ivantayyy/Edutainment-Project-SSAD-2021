using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string versionName = "0.2";
    public int mode = 0; //mode 0 = singleplayer, 1 = multiplayer, 2 = custom, 3 = assignment
    // Start is called before the first frame update
    public GameObject leaderboard, main;
    public GameObject backbtn;

    public static MainMenu instance;
    private void Awake()
    {
        if (instance == null)
        {
            Debug.Log("Main Menu Manager instantiated");
            DontDestroyOnLoad(transform.gameObject);
            //connect to photon
            PhotonNetwork.ConnectUsingSettings(versionName);
            mainUI();
            instance = this;
        }
        
    }

    public async void updateButton()
    {
        await FirebaseManager.updateScoreOnDatabaseAsync("singlePlayer", "BDE01kF5WGVzIVO0RnNlYeGlsgn1", 1, 150F, 15F);
        Debug.Log("successfully updated");
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
    public void mainUI()
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
    }

    

}
