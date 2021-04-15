using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    /**
     * Controls the animations for the quiz objects and handles user interactions with the quizzes.
     */
    public class QuestionSign : MonoBehaviour
    {
        public GameObject WeaponQuiz; //consist of quizManager and submit button for each quiz
        public GameObject Event; //event consist of quiz texts, images and animations
        public GameObject[] options;
        public GameObject[] animObj;
        private Animator[] anim;
        public bool playerInRange;
        public int correct;
        public string quizName;
        public AbstractQuizManager quizManager;

        /**
         * Start() is called before the first frame.
         * Instantiates the animator of the quiz object.
         */
        void Start()
        {

            anim = new Animator[animObj.Length];
            restartQuiz(quizManager);
        }

        /**
         * Update is called once per frame.
         * If player is near a sign and presses spacebar, enables dialog box of quiz object.
         * If dialog box is already active, it is disabled when spacebar is pressed.
         */
        void Update()
        {
            correct = quizManager.getNumCorrect();
            if (Input.GetKeyDown(KeyCode.Space) && playerInRange) //if player near sign and press spacebar
            {
                if (Event.activeInHierarchy) //if dialog box already active, disable dialog
                {
                    Event.SetActive(false);
                    WeaponQuiz.SetActive(false);
                    quizManager.enabled = false;
                    for (int i = 0; i < options.Length; i++)
                    {
                        options[i].SetActive(false);
                    }
                }
                else                            //if dialog box is not active, enable dialog box
                {
                    Event.SetActive(true);
                    WeaponQuiz.SetActive(true);
                    quizManager.enabled = true;
                    for (int i = 0; i < options.Length; i++)
                    {
                        options[i].SetActive(true);
                    }
                    restartQuiz(quizManager);
                    //dialogText.text = dialog;
                }
            }
        }

        /**
         * Sets off trigger when player is in range of question sign game object.
         */
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))  //if sign's box collider collides with player's box collider (entering)
            {
                playerInRange = true;
                //Debug.Log("Player in range");
                restartQuiz(quizManager);
            }
        }

        /**
         * Sets off trigger when player leaves the range of question sign game object.
         */
        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player")) //if sign's box collider collides with player's box collider (exiting)
            {
                playerInRange = false;
                WeaponQuiz.SetActive(false);
                quizManager.enabled = false;
                //Debug.Log("Player left range");
                Event.SetActive(false);
                for (int i = 0; i < options.Length; i++)
                {
                    options[i].SetActive(false);
                }
            }
        }

        /**
         * Restarts quiz when player presses spacebar to disable and enable the dialog box of a quiz object again.
         */
        public void restartQuiz(AbstractQuizManager quizManager)
        {
            //Debug.Log("correct = " + correct);
            //Debug.Log("qna = " + quizManager.QnA.Count);
            //if not all questions correct, restart quiz and animation
            if (correct < quizManager.QnA.Count)
            {
                //restart animation
                for (int i = 0; i < animObj.Length; i++)
                {
                    anim[i] = animObj[i].GetComponent<Animator>();
                    anim[i].Play("default");
                    anim[i].Play("default" + i);
                }
                //restart quiz
                quizManager.setNumCorrect(0);
                quizManager.currentQuestion = 0;
                quizManager.generateQuestion();
            }
        }

    }

}