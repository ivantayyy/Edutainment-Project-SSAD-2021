using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DBQT
{
    public string Answer;
    public string Options;
    public string Question;

    public DBQT()
    {
        Question = LoadQuestions.Question;
        Answer = LoadQuestions.Answer;
        Options = LoadQuestions.Options;

    }
}
