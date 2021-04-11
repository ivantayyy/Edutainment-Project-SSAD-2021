using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MCQData
{
    public string Options;
    public string Question;
    public string Answer;
    

    public MCQData()
    {
        Question = MCQCreation.question;
        Answer = MCQCreation.answer;
        Options = MCQCreation.options;
        
    }
}
