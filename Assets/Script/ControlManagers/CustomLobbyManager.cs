using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Assets
{

    /**
     * CustomLobbyManager manages the Custom Lobby page, handles user action including 'Create Custom Lobby' and 'Join Room'.
     */
    public class CustomLobbyManager : MonoBehaviour
    {
        [SerializeField] private InputField joinGameInput;

        /**
         * Loads custom lobby creation scene for player to create custom questions
         */
        public void createCustom()
        {
            PhotonNetworkMngr.loadLevel("CustomLobbyQuestionCreation");
        }

        /**
         * Joins a photon network room from input text.
         */
        public void joinCustom()
        {
            PhotonNetworkMngr.joinRoom(joinGameInput.text, new RoomOptions() { MaxPlayers = 2 }, "Lobby");
        }

        /**
         * Returns user to main menu
         */
        public void backButton()
        {
            Destroy(GameObject.Find("modeObject"));
            PhotonNetworkMngr.loadLevel("Main Menu");
        }

    }

}
