using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreElement : MonoBehaviour
{
    public Text usernameText;
    public Text scoreText;

    public void NewScoreElement(string _username, float _score)
    {
        usernameText.text = _username;
        scoreText.text = _score.ToString();
    }
}
