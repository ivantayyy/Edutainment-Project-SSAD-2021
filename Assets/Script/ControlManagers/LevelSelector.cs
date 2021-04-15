using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Assets
{

    /**
     * LevelSelector loads all the level options, but only allows users to select levels that has been unlocked.
     */
    public class LevelSelector : MonoBehaviour
    {


        private int mode;
        private GameObject modeObject;
        public Button[] levelButtons;

        /**
         * Gets maximum level reached by user, to allow user to only select levels equal or lower that the maximum reached.
         */
        async void Start()
        {
            modeObject = GameObject.Find("modeObject");
            mode = modeObject.GetComponent<mode>().modeType;
            int levelSelect;
            //multiplayer
            if(mode == 1 ||mode == 2)
            {
                levelSelect = 15;
                foreach (var photonPlayer in PhotonNetwork.playerList)
                {
                    Debug.Log("Photon player list = " + PhotonNetwork.playerList);
                    string userid = photonPlayer.UserId;
                    var task = FirebaseManager.getUserMaxLevelReachedAsync("multiPlayer");
                    int level = await task;
                    Debug.Log(userid + " " + level);
                    if (level <= levelSelect)
                    {
                        levelSelect = level;
                    }
                }
            }
            else {
                var levelReachedTask = FirebaseManager.getUserMaxLevelReachedAsync("singlePlayer");
                levelSelect = await levelReachedTask;
                UnityEngine.Debug.Log("sucess level reached " + levelSelect.ToString());
            }
            PlayerPrefs.GetInt("levelReached", levelSelect);
            int currentLevel = PlayerPrefs.GetInt("currentLevel", 1);


            //Debug


            Debug.Log(PlayerPrefs.GetInt("levelReached"));
            Debug.Log(levelSelect);
            for (int i = 0; i < levelButtons.Length; i++)
            {
                if (i + 1 > levelSelect)
                    levelButtons[i].interactable = false;
            }
        }

        /**
         * Brings user back to Character Selection scene.
         */
        public void backButton()
        {
            Destroy(GameObject.Find("modeObject"));
            PhotonNetworkMngr.leaveRoom();
        }

        /**
         * Brings user back to Main Menu
         */ 
        void OnLeftRoom()
        {
            PhotonNetworkMngr.loadLevel("Main Menu");
        }

        public void Select1(string levelName)
        {
            PlayerPrefs.SetInt("currentLevel", 1);
            PhotonNetworkMngr.loadLevel(levelName);
        }
        public void Select2(string levelName)
        {
            PlayerPrefs.SetInt("currentLevel", 2);
            PhotonNetworkMngr.loadLevel(levelName);
        }
        public void Select3(string levelName)
        {
            PlayerPrefs.SetInt("currentLevel", 3);
            PhotonNetworkMngr.loadLevel(levelName);
        }
        public void Select4(string levelName)
        {
            PlayerPrefs.SetInt("currentLevel", 4);
            PhotonNetworkMngr.loadLevel(levelName);
        }
        public void Select5(string levelName)
        {
            PlayerPrefs.SetInt("currentLevel", 5);
            PhotonNetworkMngr.loadLevel(levelName);
        }
        public void Select6(string levelName)
        {
            PlayerPrefs.SetInt("currentLevel", 6);
            PhotonNetworkMngr.loadLevel(levelName);
        }
        public void Select7(string levelName)
        {
            PlayerPrefs.SetInt("currentLevel", 7);
            PhotonNetworkMngr.loadLevel(levelName);
        }
        public void Select8(string levelName)
        {
            PlayerPrefs.SetInt("currentLevel", 8);
            PhotonNetworkMngr.loadLevel(levelName);
        }
        public void Select9(string levelName)
        {
            PlayerPrefs.SetInt("currentLevel", 9);
            PhotonNetworkMngr.loadLevel(levelName);
        }
        public void Select10(string levelName)
        {
            PlayerPrefs.SetInt("currentLevel", 10);
            PhotonNetworkMngr.loadLevel(levelName);
        }
        public void Select11(string levelName)
        {
            PlayerPrefs.SetInt("currentLevel", 11);
            PhotonNetworkMngr.loadLevel(levelName);
        }
        public void Select12(string levelName)
        {
            PlayerPrefs.SetInt("currentLevel", 12);
            PhotonNetworkMngr.loadLevel(levelName);
        }
        public void Select13(string levelName)
        {
            PlayerPrefs.SetInt("currentLevel", 13);
            PhotonNetworkMngr.loadLevel(levelName);
        }
        public void Select14(string levelName)
        {
            PlayerPrefs.SetInt("currentLevel", 14);
            PhotonNetworkMngr.loadLevel(levelName);
        }
        public void Select15(string levelName)
        {
            PlayerPrefs.SetInt("currentLevel", 15);
            PhotonNetworkMngr.loadLevel(levelName);
        }
    }

}
