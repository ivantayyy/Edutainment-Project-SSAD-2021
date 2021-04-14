using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Assets
{
    /**
    *  Scorepersec controls the increase in the enemy's score when the enemy reaches a quiz object.
    */
    public class Scorepersec : MonoBehaviour
    {
        public Text scoreText;
        public float scoreAmount;
        public float pointIncreased;
        private float timer;
        private static bool isWaiting; // reference to variable in Enemy script

        /**
        *  Start() is called before the first frame update.
        *  It sets the score as 0 and point increment as 1.
        */
        void Start()
        {
            scoreAmount = 0f;
            pointIncreased = 1f;

        }

        /**
        *  Update() is called once per frame.
        *  It increases the enemy's score by 1 every 8 seconds when the enemy is at a quiz object.
        *  The enemy's score is only increased when the enenmy is at a quiz object.
        */
        void Update()
        {
            isWaiting = Enemy.isWaiting;
            timer += Time.deltaTime;
            scoreText.text = $"EnemyScore: {(int)scoreAmount}";
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
                        scoreText.text = scoreText.ToString();
                    }
                }
                timer = 0;
            }
        }
    }

}
