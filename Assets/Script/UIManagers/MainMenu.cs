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
    private List<string> assignmentList;

    public static MainMenu instance;
    private void Awake()
    {
       
        Debug.Log("Main Menu Manager instantiated");
        DontDestroyOnLoad(transform.gameObject);
        //connect to photon
        PhotonNetworkMngr.connectUsingSettings(versionName);

        
    }

    public async void Start()
    {
        //string uid = PhotonNetwork.player.UserId;
        //await FirebaseManager.updateAssignmentScoreAsync(uid,"AssignmentID",12);

    }

    private void OnConnectedToMaster()//when connected to photon netwoek
    {
        PhotonNetworkMngr.joinLobby(TypedLobby.Default);//define lobby tyoe of photon
        Debug.Log("Connected");
    }
    public void singlePlayer()//link to single player button
    {
        //when select sinngleplayer, go to singple player page
        this.mode = 0;
        Debug.Log("mode = " + mode);
        //LOAD LEVEL IN PHOTON
        PhotonNetworkMngr.loadLevel("ChooseCharacters");
    }
    public void multiPlayer()
    {
        this.mode = 1;
        
        Debug.Log("mode = " + mode);
        PhotonNetworkMngr.loadLevel("Multiplayer");
    }
    public void custom()
    {
        this.mode = 2;
        Debug.Log("mode = " + mode);
        PhotonNetworkMngr.loadLevel("CustomLobby");
    }
    public void assignment()
    {
        this.mode = 3;
        PhotonNetworkMngr.loadLevel("Assignment");
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
    }
    */
    

}
