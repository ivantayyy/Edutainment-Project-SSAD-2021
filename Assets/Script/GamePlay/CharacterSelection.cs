using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    // Start is called before the first frame update
    public SelectedCharacter sel;
    private Hashtable playerProperties = new Hashtable();
    public GameObject[] playerText;
    public GameObject[] readyText;
    public GameObject startButton;
    public GameObject readyButton;
    private bool readyState = false;
    private GameObject mainMenuScript;
    private int mode;
    public GameObject displayText;

    private string player1_id;
    private string player2_id;

    private void Awake()
    {
        //find do not destroy object and get values
        mainMenuScript = GameObject.Find("MainMenuScript");
        mode = mainMenuScript.GetComponent<MainMenu>().mode;

        if (mode == 1 || mode == 2)//if multiplayer or custom mode
        {
            //set all player's playerReady property to false
            for (int i = 0; i < PhotonNetworkMngr.checkPlayerListLength(); i++)
            {
                playerProperties["PlayerReady"] = false;
                PhotonNetworkMngr.setPlayerPropertiesForCurrentPlayer(playerProperties);
            }
        }
  

    }
    public void Start()
    {
       
        //scenes will sync for all photon players
        PhotonNetworkMngr.setAutomaticallySyncScene(true);

        //playerProperties.Add("PlayerReady", readyState);
        //check add UserId to the player
        string userid = FirebaseManager.auth.CurrentUser.UserId;
        UnityEngine.Debug.Log(userid);
        PhotonNetworkMngr.setUserId(userid);

        string username = FirebaseManager.auth.CurrentUser.DisplayName;
        UnityEngine.Debug.Log(username);
        //Photon Netwrok is a static class
        //Set player name
        PhotonNetworkMngr.setNickName(username);
    }

    public void Update()
    {
        checkInputs();
    }

    public void checkInputs()
    {
        //ready button is hidden until player chooses a character
        if (sel.selection != "")
            readyButton.SetActive(true);

        if (mode == 1 || mode == 2) // if multiplayer or custom game
        {
            //if all players are ready and player is master client, show start button
            if (allPlayersReady() && PhotonNetworkMngr.checkIsMasterClient())
            {
                startButton.SetActive(true);
            }
            //check when a player is ready and display ready text under name
            for (int i = 0; i < PhotonNetworkMngr.checkPlayerListLength(); i++)
            {
                playerText[i].SetActive(true);
                if ((bool)PhotonNetworkMngr.getPlayerPropertyForSpecificPlayer(PhotonNetworkMngr.getPlayerFromPlayerlist(i),"PlayerReady"))
                {
                    readyText[PhotonNetworkMngr.getPlayerFromPlayerlist(i).ID - 1].SetActive(true);
                }
                else
                {
                    readyText[PhotonNetworkMngr.getPlayerFromPlayerlist(i).ID - 1].SetActive(false);
                }
            }
        }
        //display character selection choice on screen
        switch (sel.selection)
        {
            case "alexis":
                displayText.GetComponent<UnityEngine.UI.Text>().text = "Alexis";

                break;

            case "chubs":
                displayText.GetComponent<UnityEngine.UI.Text>().text = "Chubs";

                break;
            case "john":
                displayText.GetComponent<UnityEngine.UI.Text>().text = "John Cena";

                break;
        }
    }

    //save character selection when player clicks button corresponding to the character
    public void aPress()
    {
        sel.selection = "alexis";    
    }
    public void cPress()
    {
        sel.selection = "chubs";
    }
    public void jPress()
    {
        sel.selection = "john";
    }


    public void startGame()
    {
        if (mode == 0)// if single player
        {
            //create and join room for single player
            RoomOptions roomOptions = new RoomOptions();
            PhotonNetworkMngr.createRoom(null, roomOptions, TypedLobby.Default);
            PhotonNetworkMngr.loadLevel("Level_select");
        }
        else //if multiplayer or custom game
        {
            //load Level_select scene
            if (PhotonNetworkMngr.checkIsMasterClient())
            {
                Debug.Log("load level");
                PhotonNetworkMngr.loadLevel("Level_select");
            }
            
        }
        
    }

    public void readyClick()
    {
        if (mode == 1 || mode == 2)// if multiplayer or custom
        {
            //set player properties "playerReady" to true
            playerProperties["PlayerReady"] = true;
            PhotonNetworkMngr.setPlayerPropertiesForCurrentPlayer(playerProperties);
        }
        else
            startButton.SetActive(true);

    }
    private bool allPlayersReady()
    {
        //check if all players have their player properties "player ready" to be true
        {
            foreach (var photonPlayer in PhotonNetwork.playerList)
            {

                //if not all players ready
                if (!(bool)PhotonNetworkMngr.getPlayerPropertyForSpecificPlayer(photonPlayer, "PlayerReady"))
                    return false;
            }

            return true;
        }
    }
    public void backButton()
    {
        //return user to main menu and leave photon network room if multiplayer or custom
        if (mode == 1|| mode == 2)
            PhotonNetworkMngr.leaveRoom();
        Destroy(GameObject.Find("MainMenuScript"));
        PhotonNetworkMngr.loadLevel("Main Menu");
    }


}
