using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageEntry : MonoBehaviour
{
    public Text StageText;
    public Text Attempt;
    public Text TimeTakenText;
    public Text PointsPerSubstageText;

    public void NewStageEntry(string StageText,string Attempt, string TimeTakenText,string points)
    {
        this.StageText.text = StageText;
        this.Attempt.text = Attempt;
        this.TimeTakenText.text = TimeTakenText;
        this.PointsPerSubstageText.text = points;
    }
}
