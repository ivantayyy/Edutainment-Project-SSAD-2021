using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class WeaponAnswer : AbstractAnswer
    {
        private string aimAnim;
        private int count;
        // Start is called before the first frame update
        void Start()
        {
            anim = new Animator[animObj.Length];
        }
        public override void playAnimation()
        {
            //play aim animation
            aimAnim = "aim" + quizManager.currentQuestion;
            anim[0] = animObj[0].GetComponent<Animator>();
            anim[0].Play(aimAnim);
            //play rock break animation
            count = quizManager.currentQuestion + 1;
            anim[count] = animObj[count].GetComponent<Animator>();
            anim[count] = animObj[count].GetComponent<Animator>();
            anim[count].Play("rock break");
        }
    }
}
