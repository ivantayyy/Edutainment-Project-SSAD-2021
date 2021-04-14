using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Assets
{
    /**
    *  Player controls the movements and animations of the player with the use of the keyboard.
    */
    public class Player : Photon.MonoBehaviour
    {
        public PhotonView photonView;
        public GameObject playerCamera;
        public Text playerName;
        public float speed;
        public Rigidbody2D myRigidbody;
        private Vector3 change;
        public Animator animator;
        public bool canMove = true;
        public Text UserName;


        public virtual void Awake()
        {
            //check add UserId to the player
            UserName.text = PhotonNetworkMngr.getPhotonPlayerNickName(photonView);

            //if the master client is me
            if (PhotonNetworkMngr.checkPhotonView(photonView))
            {
                playerCamera.SetActive(true);
            }


        }

        /**
        *  Start() is called before the first frame update.
        *  It gets the Animator component and Rigidbody2D component from the player object.
        */
        public virtual void Start()
        {
            animator = GetComponent<Animator>();        //animation for player
            myRigidbody = GetComponent<Rigidbody2D>();  //rigidbody for player
        }

        /**
        *  Update() is called once per frame.
        *  It calls the checkInput() function when photonView.isMine && canMove == true.
        */
        public virtual void Update()
        {
            if (photonView.isMine && canMove == true)
            {
                checkInput();
            }

        }

        /**
        *  This function gets the x and y axis movements of the player and calls UpdateAnimationAndMove() to change the players animations and movements.
        */
        public void checkInput()
        {
            change = Vector3.zero;      //resets placement to zero every update
                                        //get x y axis movement
            change.x = Input.GetAxisRaw("Horizontal");
            change.y = Input.GetAxisRaw("Vertical");
            UpdateAnimationAndMove();
        }

        /**
        *  This function updates the players animations and movements according to the keyboard input.
        */
        void UpdateAnimationAndMove()   //player animation for movement
        {
            if (change != Vector3.zero) //if player is moving
            {
                MoveCharacter();
                animator.SetFloat("moveX", change.x);
                animator.SetFloat("moveY", change.y);
                animator.SetBool("moving", true);
            }
            else
            {
                animator.SetBool("moving", false);
            }
        }

        /**
        *  This function moves the player's position on the screen.
        */
        void MoveCharacter()
        {
            //player movement
            myRigidbody.MovePosition(
                transform.position + change * speed * Time.deltaTime
                );
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "bullet")
            {
                if (collision.gameObject != null) { Debug.Log("lame"); }
                Vector3 movDir = this.myRigidbody.transform.position - collision.transform.position;
                transform.position = transform.position + movDir;

                Destroy(collision.gameObject);

            }
            if (collision.gameObject.tag == "kick")
            {
                Debug.Log("lame");
                animator.SetBool("isKicked", true);
                canMove = false;
                StartCoroutine(stun());



            }
        }

        IEnumerator stun()
        {
            yield return new WaitForSeconds(3);
            animator.SetBool("isKicked", false);
            canMove = true;
            Debug.Log("3seconds");
        }



    }

}
