using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendingQuestions : MonoBehaviour
{
    public AbstractQuizManager[] quizManagers;
    public GameObject[] pendingIcons;
    void Update()
    {
        for (int i=0; i<quizManagers.Length;i++)
        {
            //if quiz completed, disable icon
            if(quizManagers[i].completed==true)
            {
                pendingIcons[i].SetActive(false);
            }
        }
    }
}
