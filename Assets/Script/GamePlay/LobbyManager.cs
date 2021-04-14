using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;
namespace Assets
{
    public class LobbyManager : MonoBehaviour
    {
        public GameObject startButton;
        private bool readyState = false;
        private Hashtable playerProperties = new Hashtable();
        public GameObject[] playerText;
        public GameObject[] ReadyText;
        public Text roomNameText;


        /**
         * Start() is called before the first frame update.
         * Syncs loading scene for all players in same photon network room
         * Add readyState to player's custom properties
         */
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

        /**
         * Coroutine to display room name, delay needed as it takes time for photonnetwork to connect
         */
        IEnumerator LateStart(float waitTime)
        {
            // coroutine to display roomname, delay needed as it takes time for photonnetwork to connect
            yield return new WaitForSeconds(waitTime);
            //Your Function You Want to Call
            roomNameText.text = "Room: " + PhotonNetworkMngr.getRoomName();
        }

        /**
         * Update is called once per frame.
         * Show start button (on master client) if all players are ready.
         * Show ready text when player is ready.
         */
        void Update()
        {
            //if all players are ready and player is master client, show start button
            if (allPlayersReady() && PhotonNetwork.isMasterClient)
            {
                startButton.SetActive(true);
            }
            //check when a player is ready and display ready text under name
            for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
            {
                playerText[i].SetActive(true);
                if ((bool)PhotonNetwork.playerList[i].CustomProperties["PlayerReady"])
                {
                    ReadyText[PhotonNetwork.playerList[i].ID - 1].SetActive(true);
                }
            }

        }

        /**
         * Function to set player properties "player ready" to true.
         */
        public void readyClick()
        {
            playerProperties["PlayerReady"] = true;
            PhotonNetwork.player.SetCustomProperties(playerProperties);
        }

        /**
         * Return user to main menu and leave photon network room
         */
        public void backButton()
        {
            Destroy(GameObject.Find("modeObject"));
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel("Main Menu");
        }

        /**
         * Load choose character scene when startbutton clicked
         */
        public void characterSelection()
        {
            PhotonNetwork.LoadLevel("ChooseCharacters");
        }

        /**
         * Check if all players have their player properties "player ready" to be true
         */
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

}
