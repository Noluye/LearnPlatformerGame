using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed, turnSpeed;
    public Transform[] patrolPoints;
    private int currentPatrolPoints;

    public Rigidbody theRB;

    private Vector3 moveDirection;
    private Vector3 lookTarget;
    private float yStore;

    private PlayerController player;

    public enum EnemyState { idle, patrolling, chasing, returning };
    public EnemyState currentState;

    public float waitTime, waitChance;
    private float waitCounter;

    public float chaseDistance, chaseSpeed, lostDistance;

    public float waitBeforeReturning;
    private float returnCounter;

    public float hopForce, waitToChase;
    private float chaseWaitCounter;

    public float waitBeforeDying = .5f;
    private bool alive = true;
    private float dyingCounter;

    public float squashSpeed;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        foreach(Transform pp in patrolPoints)
        {
            pp.parent = null;
        }

        currentState = EnemyState.idle;
        waitCounter = waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive)
        {
            dyingCounter -= Time.deltaTime;
            theRB.velocity = new Vector3(0f, theRB.velocity.y, 0f);


            transform.localScale = Vector3.Lerp(
                transform.localScale, 
                new Vector3(1.25f, .5f, 1.25f), 
                squashSpeed * Time.deltaTime);

            if (dyingCounter < 0) Destroy(gameObject);
            return;
        }

        switch(currentState)
        {
            case EnemyState.idle:
                yStore = theRB.velocity.y;
                theRB.velocity = new Vector3(0, yStore, 0);

                waitCounter -= Time.deltaTime;
                if (waitCounter <= 0)
                {
                    currentState = EnemyState.patrolling;
                    NextPatrolPoint();
                }
                break;
            case EnemyState.patrolling:
                yStore = theRB.velocity.y;
                moveDirection = patrolPoints[currentPatrolPoints].position - transform.position;
                moveDirection.y = 0f;
                moveDirection.Normalize();

                theRB.velocity = moveDirection * moveSpeed;
                theRB.velocity = new Vector3(theRB.velocity.x, yStore, theRB.velocity.z);

                if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPoints].position) <= .8f)
                {
                    NextPatrolPoint();
                }
                else
                {
                    lookTarget = patrolPoints[currentPatrolPoints].position;
                }
                break;
            case EnemyState.chasing:
                lookTarget = player.transform.position;
                if (chaseWaitCounter > 0)
                {
                    chaseWaitCounter -= Time.deltaTime;
                }
                else
                {
                    yStore = theRB.velocity.y;
                    moveDirection = player.transform.position - transform.position;
                    moveDirection.y = 0f;
                    moveDirection.Normalize();

                    theRB.velocity = moveDirection * chaseSpeed;
                    theRB.velocity = new Vector3(theRB.velocity.x, yStore, theRB.velocity.z);
                }
                if (Vector3.Distance(player.transform.position, transform.position) > lostDistance)
                {
                    currentState = EnemyState.returning;
                    returnCounter = waitBeforeReturning;
                }
                break;
            case EnemyState.returning:
                returnCounter -= Time.deltaTime;
                if (returnCounter <= 0)
                {
                    currentState = EnemyState.patrolling;
                }
                break;

        }

        if (currentState != EnemyState.chasing)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < chaseDistance)
            {
                currentState = EnemyState.chasing;
                theRB.velocity = Vector3.up * hopForce;
                chaseWaitCounter = waitToChase;
            }
        }

        lookTarget.y = transform.position.y;
        //transform.LookAt(lookTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, 
            Quaternion.LookRotation(lookTarget - transform.position), 
            turnSpeed * Time.deltaTime);
    }

    public void NextPatrolPoint()
    {
        if (Random.Range(0f, 100f) < waitChance)
        {
            waitCounter = waitTime;
            currentState = EnemyState.idle;
        }
        else
        {
            currentPatrolPoints++;
            if (currentPatrolPoints >= patrolPoints.Length) currentPatrolPoints = 0;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (alive && collision.gameObject.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer();
            chaseWaitCounter = waitToChase;  // give the palyer the chance to get rid of the enemy
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (alive && collision.gameObject.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer();
            chaseWaitCounter = waitToChase;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            alive = false;
            dyingCounter = waitBeforeDying;
            player.Bounce();
        }
    }
}
