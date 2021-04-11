using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class John : Player
{

    public GameObject upKick, downKick, leftKick, rightKick;
    private AnimatorClipInfo[] CurrentClipInfo;
    private string ClipName;

    // Start is called before the first frame update
     public override void Awake()
    {
        base.Awake();
       
    }

    public override void Start()
    {
        base.Start();
        
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (photonView.isMine)
        {
            if (Input.GetMouseButtonDown(0))
            {

                animator.SetBool("kick", true);

            }
            if (Input.GetMouseButtonUp(0))
            {
                animator.SetBool("kick", false);

            }
            CurrentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
            ClipName = CurrentClipInfo[0].clip.name;
            switch (ClipName)
            {
                case "kickRight":
                    rightKick.SetActive(true);
                    break;
                case "kickLeft":
                    leftKick.SetActive(true);
                    break;
                case "kickUp":
                    upKick.SetActive(true);
                    break;
                case "kickDown":
                    downKick.SetActive(true);
                    break;
                default:
                    rightKick.SetActive(false);
                    leftKick.SetActive(false);
                    upKick.SetActive(false);
                    downKick.SetActive(false);

                    break;
            }
        }
        

    }
    
    

}
