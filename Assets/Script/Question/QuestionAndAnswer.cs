using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionAndAnswer
{
    public string Questions;
    public string[] Answers;
    public string CorrectAnswer;

    public QuestionAndAnswer(string question, string[] answers, string correctAns)
    {
        Questions = question;
        Answers = answers;
        CorrectAnswer = correctAns;
    }
}