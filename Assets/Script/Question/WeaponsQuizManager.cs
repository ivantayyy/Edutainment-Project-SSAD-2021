using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class WeaponsQuizManager : AbstractQuizManager
    {
        public List<QuestionAndAnswer> QnA1;
        /*public List<QuestionAndAnswer> QnA;
        public GameObject[] options;
        public int currentQuestion;
        public Text QuestionTxt;
        private int numCorrect = 0;
        public Text ScoreTxt;*/
        public Toggle[] toggle;
        public ToggleGroup toggleGroup;
        public QuestionAndAnswer[] questions = new QuestionAndAnswer[3];



        /**
         * Start() is called before the first frame update.
         */
        void Start()
        {
            generateQuestion();
        }

        /*
        void Update()
        {
            ScoreTxt.text = numCorrect + "/" + QnA.Count;
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
                for (int i = 0; i < options.Length; i++)
                {
                    options[i].SetActive(false);
                }

            }
            else
            {
                //SetAnswers();
                checkAns();
            }

        }
        /*public void correct()
        {
            numCorrect++;
            currentQuestion++;
            if (currentQuestion < QnA.Count)
             {
                 //QnA.RemoveAt(currentQuestion);  
                 generateQuestion();
             }       
        }

        public void wrong()
        {
            currentQuestion++;
            if (currentQuestion < QnA.Count)
            {
                //QnA.RemoveAt(currentQuestion);
                generateQuestion();
            }
        }

        public void generateQuestion()
        {
            //currentQuestion = Random.Range(0, QnA.Count);
            Debug.Log(currentQuestion);
            QuestionTxt.text = QnA[currentQuestion].Questions;

            SetAnswers();
        }
        */
        public override void checkAns()
        {
            int correct = int.Parse(QnA[currentQuestion].CorrectAnswer);
            //Debug.Log("correct1 = " + QnA[currentQuestion].CorrectAnswer);
            for (int i = 0; i < toggle.Length; i++)
            {
                if (toggle[correct].isOn)
                    options[0].GetComponent<WeaponAnswer>().isCorrect = true;
                else if (!toggle[correct].isOn)
                    options[0].GetComponent<WeaponAnswer>().isCorrect = false;
            }

        }


        public override void SetAnswers()
        {

            options[0].GetComponent<WeaponAnswer>().isCorrect = false;
            for (int i = 0; i < toggle.Length; i++)
            {
                toggle[i].transform.GetChild(1).GetComponent<Text>().text = QnA[currentQuestion].Answers[i];
            }

        }

    }
}