using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;


public class lobbyController : MonoBehaviour
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

        PhotonNetwork.automaticallySyncScene = true;
        playerProperties.Add("PlayerReady", readyState);
        PhotonNetwork.player.SetCustomProperties(playerProperties);
        Debug.Log((bool)PhotonNetwork.player.CustomProperties["PlayerReady"]);

        StartCoroutine(LateStart(1.5f));

    }
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //Your Function You Want to Call
        roomNameText.text = "Room: " + PhotonNetwork.room.Name;
    }

    // Update is called once per frame
    void Update()
    {
        if (allPlayersReady()&&PhotonNetwork.isMasterClient)
        {
            startButton.SetActive(true);
        }  
        for (int i=0;i< PhotonNetwork.playerList.Length; i++)
        {
            playerText[i].SetActive(true);
            Debug.Log(PhotonNetwork.player.ID);
            if ((bool)PhotonNetwork.playerList[i].CustomProperties["PlayerReady"])
            {
                ReadyText[PhotonNetwork.playerList[i].ID - 1].SetActive(true);
            }
        }

    }

    public void readyClick()
    {
        playerProperties["PlayerReady"] = true;
        PhotonNetwork.player.SetCustomProperties(playerProperties);
        
    }
    public void backButton()
    {
        PhotonNetwork.LoadLevel("Main Menu");
    }

    public void characterSelection()

    {
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("load level");

            PhotonNetwork.LoadLevel("ChooseCharacters"); 

        }
    }
    private bool allPlayersReady()
    {
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
