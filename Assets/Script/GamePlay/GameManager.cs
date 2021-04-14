using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject alexis;
    public GameObject chubs;
    public GameObject john;
    public GameObject enemy;
    public GameObject sceneCamera;
    public Text pingText;
    public string selection;
    private GameObject sel;
    float random;
    private GameObject mainMenuScript;
    private int mode;
    private void Awake()
    {
        mainMenuScript = GameObject.Find("MainMenuScript");
        mode = mainMenuScript.GetComponent<MainMenu>().mode;
        sel = GameObject.Find("SelectedCharacter");
        SelectedCharacter s = sel.GetComponent<SelectedCharacter>();
        this.selection = s.selection;


    }
    private void Start()
    {

        Debug.Log("GameManager start");
        spawnPlayer();
        if (mode == 0)
            spawnEnemy();
    }
    public void Update()
    {
        pingText.text = "Ping:" + PhotonNetworkMngr.getPing();

    }

    public void spawnEnemy()
    {
        PhotonNetworkMngr.instantiatePlayer(enemy.name, new Vector2(18, 15), Quaternion.identity, 0);
    }
    public void spawnPlayer()
    {
        switch (this.selection)
        {
            case "alexis":
                Debug.Log("Spawn Player");
                random = Random.Range(-1f, 1f);
                PhotonNetworkMngr.instantiatePlayer(alexis.name, new Vector2(11, 14), Quaternion.identity, 0);  
                sceneCamera.SetActive(true);

                break;

            case "chubs":
                Debug.Log("Spawn Player");
                random = Random.Range(-1f, 1f);
                PhotonNetworkMngr.instantiatePlayer(chubs.name, new Vector2(17, 15), Quaternion.identity, 0);

                sceneCamera.SetActive(true);
                //character = Instantiate(chubs) as GameObject;
                // character.transform.position = new Vector2(0f, 0f);
                break;
            case "john":
                Debug.Log("Spawn Player");
                random = Random.Range(-1f, 1f);
                PhotonNetworkMngr.instantiatePlayer(john.name, new Vector2(0, 0), Quaternion.identity, 0);
                sceneCamera.SetActive(true);
                //character = Instantiate(john) as GameObject;
                //character.transform.position = new Vector2(0f, 0f);
                break;
                //  }

        }

    }
}
   