using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class TrashQuizManager : AbstractQuizManager
    {
        public GameObject inputField;
        private string inputAns;
        public bool correct = false;
        public QuestionAndAnswer[] questions = new QuestionAndAnswer[4];

        /**
         * Start is called before the first frame update.
         * Generates question
         */
        void Start()
        {
            /*questions[0] = new QuestionAndAnswer("Choose A", new string[1] { "A" }, 0);
            questions[1] = new QuestionAndAnswer("Choose B", new string[1] { "B"}, 0);
            questions[2] = new QuestionAndAnswer("Choose C", new string[1] { "C"}, 0);
            questions[3] = new QuestionAndAnswer("Choose D", new string[1] { "D" }, 0);
            for (int i = 0; i < 4; i++)
            {
                QnA.Add(questions[i]);
            }*/
            generateQuestion();
        }

        /**
         * Displays score.
         * Displays 'Try Again' if quiz is incomplete. 
         * Displays 'Mission Completed' if quiz is complete.
         */
        void Update()
        {
            //display score
            ScoreTxt.text = numCorrect + "/" + QnA.Count;
            //display try again/mission completed at end of quiz depending on score
            if (currentQuestion >= QnA.Count)
            {
                if (numCorrect < QnA.Count)
                {
                    QuestionTxt.text = "Try Again";
                }
                else
                {
                    QuestionTxt.text = "Mission Completed";
                    completed = true;
                }
                options[0].SetActive(false);
            }
            else
            {
                //check input for correct answer
                SetAnswers();
            }
        }

        /**
         * Check if answer is correct.
         */
        public override void checkAns()
        {
            //check answer with inputfield
            inputAns = inputField.GetComponent<InputField>().text;

            if (inputAns.CompareTo(QnA[currentQuestion].CorrectAnswer) == 0)
            {
                correct = true;
            }
            else
                correct = false;
        }

        /**
         * Set button isCorrect to true if answer is correct
         */
        public override void SetAnswers()
        {
            checkAns();

            if (correct)
            {
                options[0].GetComponent<TrashAnswer>().isCorrect = true;
            }
            else
            {
                options[0].GetComponent<TrashAnswer>().isCorrect = false;
            }
        }

    }
}