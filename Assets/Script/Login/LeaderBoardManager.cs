using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardManager : MonoBehaviour
{
    //LeaderBoard variables
    public static LeaderBoardManager instance;
    [Header("LeaderBoard")]
    public GameObject scoreElement;
    public Transform scoreboardCustomContent;
    public Transform scoreboardMultiContent;
    public Transform scoreboardSingleContent;
    // Start is called before the first frame update


    void Start()
    {
        if(instance == null)
        {
            loadAllLeaderBoards();
            instance = this;
        }
    }
    public void loadAllLeaderBoards()
    {
        LoadLeaderBoardData("customPlayer", scoreboardCustomContent);
        LoadLeaderBoardData("multiPlayer", scoreboardMultiContent);
        LoadLeaderBoardData("singlePlayer", scoreboardSingleContent);
    }
    async public void LoadLeaderBoardData(string gameMode, Transform scoreboardContent)
    {
        //Get all the users data ordered by score amount
        List<InitUser> LeaderBoardUsers = new List<InitUser>();
        LeaderBoardUsers = await FirebaseManager.GetLeaderBoardFromDatabase(gameMode);
        foreach (Transform child in scoreboardContent.transform)
        {
            UnityEngine.Debug.Log("reached before transform loop");
            Destroy(child.gameObject);
            UnityEngine.Debug.Log("reached after transform loop");
        }
        //Loop through every users UID
        foreach (InitUser user in LeaderBoardUsers)
        {
            string username = user.username;
            float totalPoints;
            if (gameMode == "customPlayer")
            {
                totalPoints = user.customPlayer.totalPoints;

            }
            else if (gameMode == "singlePlayer")
            {
                totalPoints = user.singlePlayer.totalPoints;

            }
            else
            {
                totalPoints = user.multiPlayer.totalPoints;
            }
            UnityEngine.Debug.Log(totalPoints.ToString());
            UnityEngine.Debug.Log(username);

            //Instantiate new scoreboard elements
            GameObject scoreBoardElement = Instantiate(scoreElement, scoreboardContent);
            scoreBoardElement.GetComponent<ScoreElement>().NewScoreElement(username, totalPoints);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
