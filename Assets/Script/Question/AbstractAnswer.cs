using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AbstractAnswer : MonoBehaviour
{
    public bool isCorrect = false;
    public AbstractQuizManager quizManager;
    public GameObject[] animObj;
    public Animator[] anim;
    private string number;

    public void answer()
    {
        if (isCorrect)
        {
            //play animation when answer correct
            playAnimation();
            Debug.Log("Correct Answer");
            //generate next question and update score
            quizManager.correct();
        }
        else
        {
            Debug.Log("Wrong Answer");
            quizManager.wrong();
        }
    }
    abstract public void playAnimation();
   
}
