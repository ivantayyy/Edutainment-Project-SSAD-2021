using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    [Serializable]

    /**
     * Format of questions in database, includes Answer, Options and Question.
     */
    public class DBQT
    {
        public string Answer;
        public string Options;
        public string Question;

        //public DBQT(string _question, string _answer, string _options)
        //{
        //    Question = _question;
        //    Answer = _answer;
        //    Options = _options;

        //}

        public DBQT()
        {
            Question = LoadQuestions.Question;
            Answer = LoadQuestions.Answer;
            Options = LoadQuestions.Options;

        }
    }
}