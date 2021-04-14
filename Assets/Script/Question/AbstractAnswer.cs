using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    abstract public class AbstractAnswer : MonoBehaviour
    {
        public bool isCorrect = false;
        public AbstractQuizManager quizManager;
        public GameObject[] animObj;
        public Animator[] anim;
        private string number;

        /**
         * Checks if the answer is correct.
         * Plays animation, generate next question and updates score when answer is correct.
         */
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
}
