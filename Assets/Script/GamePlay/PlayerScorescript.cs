using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerScorescript : MonoBehaviour

{
    public int scoreValue = 0;
    public AbstractQuizManager MCQ1;
    public AbstractQuizManager MCQ2;
    public AbstractQuizManager MCQ3;
    public AbstractQuizManager SAQ1;
    public AbstractQuizManager SAQ2;

    private Hashtable playerProperties = new Hashtable();

    public Text player2ScoreText;
    public Text player1ScoreText;
    private float scoreAmount;
    private float pointIncreased;
    private float timer;
    private static bool isWaiting; // reference to variable in Enemy script

    private int MCQ1Score = 0;
    private int MCQ2Score = 0;
    private int MCQ3Score = 0;
    private int SAQ1Score = 0;
    private int SAQ2Score = 0;

    private int mode;
    private GameObject mainMenuScript;

    // Start is called before the first frame update
    void Start()
    {
        scoreAmount = 0f;
        pointIncreased = 1f;

        //get is multiplayer bool
        mainMenuScript = GameObject.Find("MainMenuScript");
        mode = mainMenuScript.GetComponent<MainMenu>().mode;
    

        playerProperties.Add("PlayerScore", scoreValue);
        PhotonNetwork.player.SetCustomProperties(playerProperties);
    }

    // Update is called once per frame
    void Update()
    {
        if(mode == 0)
            enemyScore();

        updatePlayerScore();      
        displayScore();
    }

    private void updatePlayerScore()
    {
        if (MCQ1.completed == true)
        {
            if (MCQ1Score == 3)
            {
                // do nothing
            }
            else
            {
                scoreValue += 3;
                MCQ1Score += 3;
            }
        }

        if (MCQ2.completed == true)
        {
            if (MCQ2Score == 3)
            {
                // do nothing
            }
            else
            {
                scoreValue += 3;
                MCQ2Score += 3;
            }
        }
        if (MCQ3.completed == true)
        {
            if (MCQ3Score == 3)
            {
                // do nothing
            }
            else
            {
                scoreValue += 3;
                MCQ3Score += 3;
            }
        }
        if (SAQ1.completed == true)
        {
            if (SAQ1Score == 3)
            {
                // do nothing
            }
            else
            {
                scoreValue += 3;
                SAQ1Score += 3;
            }
        }
        if (SAQ2.completed == true)
        {
            if (SAQ2Score == 3)
            {
                // do nothing
            }
            else
            {
                scoreValue += 3;
                SAQ2Score += 3;
            }
        }
        playerProperties["PlayerScore"] = scoreValue;
        PhotonNetwork.player.SetCustomProperties(playerProperties);
        player1ScoreText.text = $"PlayerScore: {scoreValue}";
        
    }

    public void enemyScore()
    {
        isWaiting = Enemy.isWaiting;
        timer += Time.deltaTime;
        player2ScoreText.text = $"EnemyScore: {(int)scoreAmount}";
        if (timer > 8f)
        {
            if (isWaiting == true) // only increment if enemy is waiting at quiz
            {
                if (scoreAmount == 15)
                {
                    scoreAmount = 15;
                }
                else
                {
                    scoreAmount += pointIncreased;
                    player2ScoreText.text = player2ScoreText.ToString();
                }
            }
            timer = 0;
        }
    }
    public void displayScore()
    {
        if(mode == 1 || mode == 2)//take score from here
        {
            player1ScoreText.text = $"Player1 Score: {PhotonPlayer.Find(1).CustomProperties["PlayerScore"]}";
            player2ScoreText.text = $"Player2 Score: {PhotonPlayer.Find(2).CustomProperties["PlayerScore"]}";
        }
        else
        {
            player1ScoreText.text = $"PlayerScore: {scoreValue}";
            player2ScoreText.text = $"EnemyScore: {(int)scoreAmount}";
        }
    }

}
