using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomLobbyManager : MonoBehaviour
{
    [SerializeField] private InputField joinGameInput;
    // Start is called before the first frame update
    public void createCustom()
    {
        PhotonNetwork.LoadLevel("CustomLobbyCreation");
    }
    public void joinCustom()
    {
        PhotonNetwork.JoinOrCreateRoom(joinGameInput.text, new RoomOptions() { MaxPlayers = 2 }, null);
        PhotonNetwork.LoadLevel("Lobby");
    }
    public void backButton()
    {
        Destroy(GameObject.Find("MainMenuScript"));
        PhotonNetwork.LoadLevel("Main Menu");
    }
}
