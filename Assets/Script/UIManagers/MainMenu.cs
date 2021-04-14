using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string versionName = "0.2";
    
    // Start is called before the first frame update
    public GameObject leaderboard, main;
    public GameObject backbtn;
    public mode modeObject;  //mode 0 = singleplayer, 1 = multiplayer, 2 = custom, 3 = assignment
    private List<string> assignmentList;

    public static MainMenu instance;
    private void Awake()
    {
       
        Debug.Log("Main Menu Manager instantiated");
        
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
        
        modeObject.modeType = 0;
        //LOAD LEVEL IN PHOTON
        PhotonNetworkMngr.loadLevel("ChooseCharacters");
    }
    public void multiPlayer()
    {
        
        modeObject.modeType = 1;
        PhotonNetworkMngr.loadLevel("Multiplayer");
    }
    public void custom()
    {
        
        modeObject.modeType = 2;
        PhotonNetworkMngr.loadLevel("CustomLobby");
    }
    public void assignment()
    {
        
        modeObject.modeType = 3;
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
