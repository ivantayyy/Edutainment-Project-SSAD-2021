using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerMenu : MonoBehaviour
{
    [SerializeField] private string versionName = "0.2";
    [SerializeField] private GameObject connectMenu;

    [SerializeField] private InputField createGameInput;
    [SerializeField] private InputField joinGameInput;
    [SerializeField] private GameObject startButton;

    /*private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(versionName);
    }

    private void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected");
    }*/

    /*public void checkCreateName()
    {
        if(createGameInput.text.Length>5)
        {
            startButton.SetActive(true);
        }
    }*/
    public void backButton()
    {
        Destroy(GameObject.Find("modeObject"));
        PhotonNetworkMngr.loadLevel("Main Menu");
    }

    public void createGame()
    {
        PhotonNetworkMngr.createRoom(createGameInput.text, new RoomOptions() { MaxPlayers = 2 }, null);
        PhotonNetworkMngr.setMasterClient(PhotonNetwork.player);
        PhotonNetworkMngr.loadLevel("Lobby");
    }

    public void joinGame()
    {
        
        PhotonNetworkMngr.joinRoom(joinGameInput.text, new RoomOptions() { MaxPlayers = 2 }, "Lobby");
    }

    private void OnJoinedRoom()
    {

    }
    
}
