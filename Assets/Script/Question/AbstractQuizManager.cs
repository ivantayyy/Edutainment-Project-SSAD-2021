using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    abstract public class AbstractQuizManager : MonoBehaviour
    {
        public List<QuestionAndAnswer> QnA = new List<QuestionAndAnswer>();
        public GameObject[] options;
        public int currentQuestion;
        public Text QuestionTxt;
        public int numCorrect = 0;
        public Text ScoreTxt;
        public bool completed = false;

        /**
         * Function to generate next question shen answer is correct.
         */
        public void correct()
        {
            {
                numCorrect++;
                currentQuestion++;
                if (currentQuestion < QnA.Count)
                {
                    //QnA.RemoveAt(currentQuestion);  
                    generateQuestion();
                }
            }
        }

        /**
         * Function to regenerate same question shen answer is wrong.
         */
        public void wrong()
        {
            currentQuestion++;
            if (currentQuestion < QnA.Count)
            {
                //QnA.RemoveAt(currentQuestion);
                generateQuestion();
            }
        }

        /**
         * Function to generate question
         */
        public void generateQuestion()
        {
            //currentQuestion = Random.Range(0, QnA.Count);
            QuestionTxt.text = QnA[currentQuestion].Questions;
            SetAnswers();
        }

        abstract public void SetAnswers();
        abstract public void checkAns();

        /**
         * Returns number of correct answers
         */
        public int getNumCorrect()
        {
            return numCorrect;
        }

        /**
         * Assigns number of correct answers to variable numCorrect.
         * @param num number of questions correct
         */
        public void setNumCorrect(int num)
        {
            numCorrect = num;
        }
    }
}