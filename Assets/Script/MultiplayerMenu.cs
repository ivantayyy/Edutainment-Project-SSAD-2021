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
        PhotonNetwork.LoadLevel("Main Menu");
    }

    public void createGame()
    {
        PhotonNetwork.CreateRoom(createGameInput.text, new RoomOptions() { maxPlayers = 2 }, null);
        PhotonNetwork.SetMasterClient(PhotonNetwork.player);
        PhotonNetwork.LoadLevel("Lobby");
    }

    public void joinGame()
    {
        
        PhotonNetwork.JoinRoom(joinGameInput.text);
        PhotonNetwork.LoadLevel("Lobby");
    }

    private void OnJoinedRoom()
    {

    }
    
}
