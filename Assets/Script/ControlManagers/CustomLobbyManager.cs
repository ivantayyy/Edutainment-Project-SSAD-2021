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
        PhotonNetworkMngr.loadLevel("CustomLobbyQuestionCreation");
    }
    public void joinCustom()
    {
        //join a photon network room from input text.
        PhotonNetworkMngr.joinRoom(joinGameInput.text, new RoomOptions() { MaxPlayers = 2 }, "Lobby");
    }
    public void backButton()
    {
        //return user to main menu
        Destroy(GameObject.Find("modeObject"));
        PhotonNetworkMngr.loadLevel("Main Menu");
    }

}
