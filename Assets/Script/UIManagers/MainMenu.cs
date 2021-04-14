using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string versionName = "0.2";
    
    // Start is called before the first frame update
    public GameObject leaderboard, main;
    public GameObject backbtn;
    public mode modeObject;  //mode 0 = singleplayer, 1 = multiplayer, 2 = custom, 3 = assignment
    private List<string> assignmentList;

    public static MainMenu instance;

    /*!\brief Awake function is called before start
     *          connect to photonNetowrk server
     */
    private void Awake()
    {
       
        Debug.Log("Main Menu Manager instantiated");
        
        //connect to photon
        

        
    }
    /*!\brief when connected to photon network, join lobby
     *          and calls instantiatePhotonUser
     */
    private void OnConnectedToMaster()//when connected to photon netwoek
    {
        PhotonNetworkMngr.joinLobby(TypedLobby.Default);//define lobby tyoe of photon
        instantiatePhotonUser();
        Debug.Log("Connected");
    }

    /*!\brief set photon userid and nickname to firebase user's userid and username
     *          
     */
    public void instantiatePhotonUser()
    {
        string userid = FirebaseManager.auth.CurrentUser.UserId;
        PhotonNetworkMngr.setUserId(userid);

        string username = FirebaseManager.auth.CurrentUser.DisplayName;
        PhotonNetworkMngr.setNickName(username);
    }
    /*!\brief go to single player page when called
     *          
     */
    public void singlePlayer()//link to single player button
    {
        //when select sinngleplayer, go to singple player page
        
        modeObject.modeType = 0;
        //LOAD LEVEL IN PHOTON
        PhotonNetworkMngr.loadLevel("ChooseCharacters");
    }
    /*!\brief go to multiplayer page when called
     *          
     */
    public void multiPlayer()
    {
        
        modeObject.modeType = 1;
        PhotonNetworkMngr.loadLevel("Multiplayer");
    }
    /*!\brief go to custom lobby page when called
     *          
     */
    public void custom()
    {
        
        modeObject.modeType = 2;
        PhotonNetworkMngr.loadLevel("CustomLobby");
    }
    /*!\brief go to student assignment page when called
     *          
     */
    public void assignment()
    {
        
        modeObject.modeType = 3;
        PhotonNetworkMngr.loadLevel("Assignment");
    }
    

}
