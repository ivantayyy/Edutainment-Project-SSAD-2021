using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Scores
{
    public int curSubstage;
    public List<float> points;
    public List<float> timeTaken;
    public List<int> attempts;
    public float totalPoints;
    //public string jsonString;
    public Scores()
    {
        //cursubstage is length of points/timetaken/attempts
        this.curSubstage = 1;
        //points for current available stage always set to 0 intially
        this.points = new List<float>()
        {
            0
        };

        this.timeTaken = new List<float>()
        {
            -1
        };
        this.attempts = new List<int>()
        {
            0
        };
        this.totalPoints = 0;
    }
}