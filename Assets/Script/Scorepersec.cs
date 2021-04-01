using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scorepersec : MonoBehaviour
{
    public Text scoreText;
    public float scoreAmount;
    public float pointIncreased;
    private float timer;
    private static bool isWaiting; // reference to variable in Enemy script

    // Start is called before the first frame update
    void Start()
    {
        scoreAmount = 0f;
        pointIncreased = 1f;

    }

    // Update is called once per frame
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
