using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class QuestionAndAnswer
    {
        public string Questions;
        public string[] Answers;
        public string CorrectAnswer;

        /**
         * @param question question
         * @param answers choices for answer
         * @param correctAnw the correct answer
         */
        public QuestionAndAnswer(string question, string[] answers, string correctAns)
        {
            Questions = question;
            Answers = answers;
            CorrectAnswer = correctAns;
        }
    }
}
