using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract public class AbstractQuizManager : MonoBehaviour
{
    public List<QuestionAndAnswer> QnA = new List<QuestionAndAnswer>();
    public GameObject[] options;
    public int currentQuestion;
    public Text QuestionTxt;
    public int numCorrect = 0;
    public Text ScoreTxt;
    public bool completed=false;

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
        QuestionTxt.text = QnA[currentQuestion].Questions;
        SetAnswers();
    }
    abstract public void SetAnswers();
    public int getNumCorrect()
    {
        return numCorrect;
    }
    public void setNumCorrect(int num)
    {
        numCorrect = num;
    }
}
