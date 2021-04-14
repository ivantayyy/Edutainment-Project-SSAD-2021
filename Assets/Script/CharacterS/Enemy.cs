using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
namespace Assets
{
    public class Enemy : MonoBehaviour
    {
        /**
        *  Enemy controls the enemy's path around different quiz objects in the single player mode. It also controls the animation of the enemy.
        */
        public string enemyName;
        public float speed;
        private int points;

        public List<GameObject> targets = new List<GameObject>();
        private GameObject nextTarget;
        int nextTargetIndex = 0;

        public float nextWayPointDistance = 3f;
        Path path;
        int currentWayPoint = 0;
        public bool reachedEndOfPath = false;
        public static bool isWaiting = false;

        Seeker seeker;
        Rigidbody2D rb;

        public Transform EnemyGFX;
        public Animator anim;

        private bool hit = false; // variable for JohnCena
        private bool slomo = false; // variable for Chubs
        private float tempspeed; // variable for Chubs

        /**
        * Start() is called before the first frame update.
        * This function adds the quiz objects as targets so that the enemy can find paths to the different quiz objects using the A* Pathfinding Algorithm.
        */
        void Start()
        {
            seeker = GetComponent<Seeker>();
            rb = GetComponent<Rigidbody2D>();
            anim = EnemyGFX.GetComponent<Animator>();
            targets.AddRange(GameObject.FindGameObjectsWithTag("QuestionSign")); // save list of QuestionSign objects
            nextTarget = targets[0];

            anim.SetBool("wakeUp", true);
            InvokeRepeating("UpdatePath", 0f, 30f);
        }

        /**
        * UpdatePath() is called when the enemy finds a target object to move towards.
        */
        void UpdatePath()
        {
            if (seeker.IsDone())
            {
                nextTargetIndex++;
                if (nextTargetIndex >= targets.Count)
                {
                    nextTargetIndex = 0;
                }
                nextTarget = targets[nextTargetIndex];
                seeker.StartPath(rb.position, nextTarget.transform.position, OnPathComplete);
            }
        }

        /**
        * OnPathComplete(Path p) is called to get a Vector3 representation of the path.
        */
        void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                path = p;
                currentWayPoint = 0;
            }
        }

        /**
        * Update() is called once per frame.
        * This updates the path when the enemy reaches the end of a path.
        * It also changes the enemy's animation when the enemy stops at a quiz object and when it starts moving towards another quiz object.
        */
        void Update()
        {
            if (path == null)
                return;
            if (currentWayPoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else
            {
                reachedEndOfPath = false;
            }

            // check if enemy reached target
            if (Vector3.Distance(transform.position, nextTarget.transform.position) <= 3.5)
            {
                isWaiting = true;
                anim.SetBool("wakeUp", false);
            }

            else
            {
                isWaiting = false;
                anim.SetBool("wakeUp", true);
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

            if (distance < nextWayPointDistance)
            {
                currentWayPoint++;
            }

            changeAnim(force);

            HitIsTrue(); // for JohnCena
            SloMoIsTrue(); // for Chubs
        }

        /**
        * This function is called to set the vectors of x and y for the enemy's animations.
        */
        private void SetAnimFloat(Vector3 setVector)
        {
            anim.SetFloat("moveX", setVector.x);
            anim.SetFloat("moveY", setVector.y);
        }

        /**
        * This function is called to change the enemy's animation according to the direction it is facing when it is moving around the map.
        */
        private void changeAnim(Vector3 direction)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                {
                    SetAnimFloat(Vector3.right);
                }
                else if (direction.x < 0)
                {
                    SetAnimFloat(Vector3.left);
                }
            }
            else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
            {
                if (direction.y > 0)
                {
                    SetAnimFloat(Vector3.up);
                }
                else if (direction.y < 0)
                {
                    SetAnimFloat(Vector3.down);
                }
            }
        }

        /**
        * This function is called when the player collides with the enemy.
        */
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("JohnCena"))
            {
                hit = true;
            }

            if (other.gameObject.CompareTag("Chubs"))
            {
                slomo = true;
            }
        }

        /**
        * This function is called when the player is "John Cena". The player will stun the enemy.
        */
        void HitIsTrue()
        {
            if (hit == true)
            {
                WaitForStunToEnd();
                hit = false;
            }
        }

        /**
        * This function is called when the player is "Chubs". The player will cause the enemy to move slower.
        */
        void SloMoIsTrue()
        {
            if (slomo == true)
            {
                tempspeed = speed;
                speed /= (float)1.5;
                WaitForSloMoToEnd();
                speed = tempspeed;
                slomo = false;
            }
        }

        /**
        * This function is called to cause the enemy to be stunned for 10 seconds if the player is "John Cena" and has collided with the enemy.
        */
        IEnumerator WaitForStunToEnd()
        {
            //wait a frame
            yield return null;
            //wait 10 seconds
            yield return new WaitForSeconds(10.0f);
        }

        /**
        * This function is called to cause the enemy to be move slower for 30 seconds if the player is "Chubs" and has collided with the enemy.
        */
        IEnumerator WaitForSloMoToEnd()
        {
            //wait a frame
            yield return null;
            //wait 10 seconds
            yield return new WaitForSeconds(30.0f);
        }
    }

}
