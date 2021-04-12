using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomLobbyManager : MonoBehaviour
{
    [SerializeField] private InputField joinGameInput;

    public void createCustom()
    {
        //loads customl lobby creation scene for player to create custom questions
        PhotonNetwork.LoadLevel("CustomLobbyCreation");
    }
    public void joinCustom()
    {
        //join a photon network room from input text.
        PhotonNetwork.JoinOrCreateRoom(joinGameInput.text, new RoomOptions() { MaxPlayers = 2 }, null);
        PhotonNetwork.LoadLevel("Lobby");
    }
    public void backButton()
    {
        //return user to main menu
        Destroy(GameObject.Find("MainMenuScript"));
        PhotonNetwork.LoadLevel("Main Menu");
    }
}
