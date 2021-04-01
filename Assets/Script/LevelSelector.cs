using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;




public class LevelSelector : MonoBehaviour
{
    public GameObject Master;
    public GameObject Client;
    public Button[] levelButtons;
    
    
    private void Awake()
    {

        //StartCoroutine(LateStart(1.2f));
        
    }
    void Start()
    {
        if(PhotonNetwork.isMasterClient)
        { int levelReached = PlayerPrefs.GetInt("levelReached", 1);
        Debug.Log(PlayerPrefs.GetInt("levelReached"));
        Debug.Log(levelReached);
            //for locking level selections
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > levelReached)
                levelButtons[i].interactable = false;
        }
        }
    }
    public void backButton()
    {
        PhotonNetwork.LoadLevel("Main Menu");
    }
    //TODO  Change from scenemanager to photonnetwaork.load
    public void Select(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
    //not used.
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (PhotonNetwork.isMasterClient)
        {
            Master.SetActive(true);
            Client.SetActive(false);
        }
        else
        {
            Master.SetActive(false);
            Client.SetActive(true);
        }

    }
}
