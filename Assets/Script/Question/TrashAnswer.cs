using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class TrashAnswer : AbstractAnswer
    {
        private int count;
        private string trashAnim;
        void Start()
        {
            anim = new Animator[animObj.Length];
        }
        public override void playAnimation()
        {
            count = quizManager.currentQuestion;
            trashAnim = "trash" + quizManager.currentQuestion;
            anim[count] = animObj[count].GetComponent<Animator>();
            anim[count].Play(trashAnim);
        }

    }
}