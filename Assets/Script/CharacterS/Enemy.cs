using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
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

    // Start is called before the first frame update
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
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }
    // Update is called once per frame
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

        Vector2 direction = ((Vector2) path.vectorPath[currentWayPoint] - rb.position).normalized;
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

    private void SetAnimFloat(Vector3 setVector)
    {
        anim.SetFloat("moveX", setVector.x);
        anim.SetFloat("moveY", setVector.y);
    }

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

    void HitIsTrue()
    {
        if (hit==true)
        {
            WaitForStunToEnd();
            hit = false;
        }
    }

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

    IEnumerator WaitForStunToEnd()
    {
        //wait a frame
        yield return null;
        //wait 10 seconds
        yield return new WaitForSeconds(10.0f);
    }

    IEnumerator WaitForSloMoToEnd()
    {
        //wait a frame
        yield return null;
        //wait 10 seconds
        yield return new WaitForSeconds(30.0f);
    }
}
