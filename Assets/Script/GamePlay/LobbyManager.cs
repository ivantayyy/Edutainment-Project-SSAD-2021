using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public GameObject startButton;
    private bool readyState = false;
    private Hashtable playerProperties = new Hashtable();
    public GameObject[] playerText;
    public GameObject[] ReadyText;
    public Text roomNameText;


    // Start is called before the first frame update
    void Start()
    {
        //sync loading scene for all players in same photonnetwork room
        PhotonNetworkMngr.setAutomaticallySyncScene(true);
        //add readystate to player's custom properties
        playerProperties.Add("PlayerReady", readyState);
        PhotonNetworkMngr.setPlayerPropertiesForCurrentPlayer(playerProperties);
        //Debug.Log((bool)PhotonNetwork.player.CustomProperties["PlayerReady"]);

        StartCoroutine(LateStart(2.5f));

    }
    IEnumerator LateStart(float waitTime)
    {
        // coroutine to display roomname, delay needed as it takes time for photonnetwork to connect
        yield return new WaitForSeconds(waitTime);
        //Your Function You Want to Call
        roomNameText.text = "Room: " + PhotonNetworkMngr.getRoomName();
    }

    // Update is called once per frame
    void Update()
    {
        //if all players are ready and player is master client, show start button
        if (allPlayersReady()&&PhotonNetwork.isMasterClient)
        {
            startButton.SetActive(true);
        }  
        //check when a player is ready and display ready text under name
        for (int i=0;i< PhotonNetwork.playerList.Length; i++)
        {
            playerText[i].SetActive(true);
            if ((bool)PhotonNetwork.playerList[i].CustomProperties["PlayerReady"])
            {
                ReadyText[PhotonNetwork.playerList[i].ID - 1].SetActive(true);
            }
        }

    }

    public void readyClick()
    {
        //when player is ready, set player properties "player ready" to true
        playerProperties["PlayerReady"] = true;
        PhotonNetwork.player.SetCustomProperties(playerProperties);
    }
    public void backButton()
    {
        //return user to main menu and leave photon network room
        Destroy(GameObject.Find("MainMenuScript"));
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Main Menu");
    }

    public void characterSelection()
    {
        //load choose character scene when startbutton clicked
        PhotonNetwork.LoadLevel("ChooseCharacters");       
    }
    private bool allPlayersReady()
    {
        //check if all players have their player properties "player ready" to be true
        {
            foreach (var photonPlayer in PhotonNetwork.playerList)
            {
                
                //if not all players ready
                if (!(bool)photonPlayer.CustomProperties["PlayerReady"])
                    return false;
            }
            return true;
        }
    }
}
