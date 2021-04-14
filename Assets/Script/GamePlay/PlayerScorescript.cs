using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
namespace Assets
{
    /**
    *  PlayerScorescript controls the score display of the player.
    */
    public class PlayerScorescript : MonoBehaviour

    {
        public int playerScore = 0;
        public AbstractQuizManager MCQ1;
        public AbstractQuizManager MCQ2;
        public AbstractQuizManager MCQ3;
        public AbstractQuizManager SAQ1;
        public AbstractQuizManager SAQ2;

        private Hashtable playerProperties = new Hashtable();

        public Text player2ScoreText;
        public Text player1ScoreText;
        public float enemyScore = 0;
        private float pointIncreased;
        private float timer;
        private static bool isWaiting; // reference to variable in Enemy script

        private int MCQ1Score = 0;
        private int MCQ2Score = 0;
        private int MCQ3Score = 0;
        private int SAQ1Score = 0;
        private int SAQ2Score = 0;

        private int mode;
        private GameObject modeObject;

        private void Awake()
        {
            //get is multiplayer bool
            modeObject = GameObject.Find("modeObject");
            mode = modeObject.GetComponent<mode>().modeType;
            Debug.Log("mode = " + mode);
        }
        // Start is called before the first frame update
        void Start()
        {
            enemyScore = 0f;
            pointIncreased = 1f;

            playerProperties.Add("PlayerScore", playerScore);
            PhotonNetworkMngr.setPlayerPropertiesForCurrentPlayer(playerProperties);
        }

        // Update is called once per frame
        void Update()
        {
            if (mode == 0)
                calculateEnemyScore();

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
                    playerScore += 3;
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
                    playerScore += 3;
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
                    playerScore += 3;
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
                    playerScore += 3;
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
                    playerScore += 3;
                    SAQ2Score += 3;
                }
            }
            playerProperties["PlayerScore"] = playerScore;
            PhotonNetworkMngr.setPlayerPropertiesForCurrentPlayer(playerProperties);
            //player1ScoreText[0].text = $"PlayerScore: {playerScore}";

        }

        /**
        *  This function increments the enemy's score when the enemy is at a quiz object.
        */
        public void calculateEnemyScore()
        {
            isWaiting = Enemy.isWaiting;
            timer += Time.deltaTime;
            player2ScoreText.text = $"EnemyScore: {(int)enemyScore}";
            if (timer > 8f)
            {
                if (isWaiting == true) // only increment if enemy is waiting at quiz
                {
                    if (enemyScore == 15)
                    {
                        enemyScore = 15;
                    }
                    else
                    {
                        enemyScore += pointIncreased;
                        player2ScoreText.text = player2ScoreText.ToString();
                    }
                }
                timer = 0;
            }
        }

        /**
        *  This function displays the scores of player1 and player2.
        */
        public void displayScore()
        {
            if (mode == 1 || mode == 2)//take score from here
            {

                for (int i = 0; i < PhotonNetworkMngr.checkPlayerListLength(); i++)
                {
                    if (PhotonNetworkMngr.checkIfPlayerIsMasterClient(PhotonNetwork.playerList[i]))
                    {
                        player1ScoreText.text = $"Player1 Score: {(int)PhotonNetworkMngr.getPlayerPropertyForSpecificPlayer(PhotonNetwork.playerList[i], "PlayerScore")}";
                    }
                    else
                        player2ScoreText.text = $"Player2 Score: {(int)PhotonNetworkMngr.getPlayerPropertyForSpecificPlayer(PhotonNetwork.playerList[i], "PlayerScore")}";
                    //player1ScoreText.text = $"{PhotonNetwork.playerList[i].NickName} Score: {PhotonNetwork.playerList[i].CustomProperties["PlayerScore"]}";  
                }
                string p1score = player1ScoreText.text;
                float playerscore = float.Parse(p1score.Substring(15, 2));
                string p2score = player2ScoreText.text;
                float enemyscore = float.Parse(p2score.Substring(15, 2));
                Debug.Log("playerscore = " + playerscore + " p2score = " + enemyscore);

                //player1ScoreText.text = $"Player1 Score: {PhotonPlayer.Find(1).CustomProperties["PlayerScore"]}";
                //player2ScoreText.text = $"Player2 Score: {PhotonPlayer.Find(2).CustomProperties["PlayerScore"]}";
            }
            else
            {
                player1ScoreText.text = $"PlayerScore: {playerScore}";
                player2ScoreText.text = $"EnemyScore: {(int)enemyScore}";
            }
        }

    }

}
