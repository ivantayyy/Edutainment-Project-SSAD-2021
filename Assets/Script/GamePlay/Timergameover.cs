using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class Timergameover : MonoBehaviour
{
    public int countDownStartValue;
    public Text timerUI;
    public Text finalText;

    public float enemyscore;
    public float playerscore;

    public GameObject PlayerScore;
    private PlayerScorescript playerScoreScript;
    private PlayerScorescript player2ScoreScript;
    public GameObject EnemyScore;
    private Scorepersec enemyScoreScript;


    private int mode;
    private GameObject modeObject;
    public GameObject gameOverUI;

    private string GameMode;

    //public int currentTime;
    // Start is called before the first frame update
    void Start()
    {
        modeObject = GameObject.Find("modeObject");
        mode = modeObject.GetComponent<mode>().modeType;

        countDownTimer();
        
        // Find the PlayerScorescript attached to "PlayerScore" for player 1
        playerScoreScript = PlayerScore.GetComponent<PlayerScorescript>();
        if (mode == 0||mode == 3) //if single player
        {
            // Find the Scorepersec script attached to "EnemyScore"
            //enemyScoreScript = EnemyScore.GetComponent<Scorepersec>();
            enemyscore = playerScoreScript.enemyScore;
            GameMode = "singlePlayer";
        }
        else if(mode==1||mode==2)//multiplayer or custom
        {
            string score = playerScoreScript.player2ScoreText.text;
            enemyscore = float.Parse(score.Substring(15,2));
            GameMode = "multiPlayer";
        }

  
    }
    
    void Update()
    {
        //playerscore = (float)PhotonPlayer.Find(1).CustomProperties["PlayerScore"];
        
        //Debug.Log("playerscore = " + playerscore);
        if (mode == 0||mode ==3) //ifsingle player
        {
            playerscore = playerScoreScript.playerScore;
            enemyscore = playerScoreScript.enemyScore;
        }
            
        else
        {
            string p1score = playerScoreScript.player1ScoreText.text;
            playerscore = float.Parse(p1score.Substring(14));
            string p2score = playerScoreScript.player2ScoreText.text;
            enemyscore = float.Parse(p2score.Substring(14));
            //Debug.Log("playerscore = " + p1score.Substring(14) + " p2score = " + p2score);
        }
    }

    void countDownTimer()
    {
        
        //currentTime = countDownStartValue;
        if (countDownStartValue > 0)
        {
            TimeSpan spanTime = TimeSpan.FromSeconds(countDownStartValue);
            timerUI.text = "Timer: " + spanTime.Minutes + " : " + spanTime.Seconds;
            
            countDownStartValue--;
            Invoke("countDownTimer", 1.0f);

            if (enemyscore == 15)
            {
                if (mode == 0 || mode == 3)// if single player
                    finalText.text = "Game over! Enemy Wins!";
                else
                    finalText.text = "Game over! Player 2 Wins!";
                countDownStartValue = 0;
                gameOverUI.SetActive(true);
            }

            if (playerscore == 15)
            {
                if (mode == 0 || mode == 3)// if single player
                    finalText.text = "Game over! Player Wins!";
                    
                else
                    finalText.text = "Game over! Player 1 Wins!";
                countDownStartValue = 0;
                gameOverUI.SetActive(true);
                //Only need to update this instance's winning player
                //UpdateWin();
            }

        }
        else
        {
            if (enemyscore > playerscore)
            {
                if (mode == 0 || mode == 3)// if single player
                    finalText.text = "Game over! Enemy Wins!";
                else
                    finalText.text = "Game over! Player 2 Wins!";

                gameOverUI.SetActive(true);
            }
            else if (playerscore > enemyscore)
            {
                if (mode == 0||mode == 3)// if single player
                    finalText.text = "Game over! Player Wins!";
                else
                    finalText.text = "Game over! Player 1 Wins!";
                gameOverUI.SetActive(true);
                //updates win asynchronously
                
                if (mode == 3)
                {
                    takeAssignmentScore();
                }
                else
                    UpdateWin();
            }
            else
            {
                finalText.text = "Game over! It's a draw!";
                gameOverUI.SetActive(true);
            }


        }
        
    }

    
    //Helper function for updateing scores when winning
    async private void UpdateWin()
    {
        //Need find a way to generate dynamically the mode type
        string uid= PhotonNetwork.player.UserId;
        int currLevelCleared = PlayerPrefs.GetInt("currentLevel",1);
        float timeTaken = 160F;
        Debug.Log("Current level cleared is: " + currLevelCleared);
        var updateTask = FirebaseManager.updateScoreOnDatabaseAsync(GameMode, uid, currLevelCleared, timeTaken, playerscore);
        await updateTask;
        if (updateTask.IsFaulted)
        {
            Debug.LogError(updateTask.Exception);
        }
        else
        {
            Debug.Log($"Successfully updated score {playerscore} for userid: {uid} to the database");
        }
    }

    async private void takeAssignmentScore()
    {
        string assignmentID = PhotonNetwork.room.Name;
        string uid = PhotonNetwork.player.NickName;
        var updateTask = FirebaseManager.updateAssignmentScoreAsync(uid, assignmentID, (int)playerscore);
        await updateTask;
        if (updateTask.IsFaulted)
        {
            Debug.LogError(updateTask.Exception);
        }
        else
        {
            Debug.Log($"Successfully updated assignment score {playerscore} for userid: {uid} to the database");
        }
    }

}
