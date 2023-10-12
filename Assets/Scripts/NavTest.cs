using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavTest : MonoBehaviour
{
    NavMeshAgent agent;

    [SerializeField] Transform target;
    [SerializeField] HandGun handGun;
    [SerializeField] FootstepsSound footStep;

    private float doorOpenDelay;
    private float dist;

    private Vector3 curPatrolSpot;
    private Vector3[] patrolSpot =
    {
        new Vector3(8.5f, .0f, -20.0f),
        new Vector3(8.5f, .0f, -32.0f),
        new Vector3(-6.8f, .0f, -28.6f),
        new Vector3(-5.0f, .0f, -44.0f),
        new Vector3(30.0f, .0f, -44.0f),
        new Vector3(18.0f, .0f, -56.0f),
        new Vector3(30.0f, .0f, -94.0f),
        new Vector3(-18.0f, .0f, -94.0f),
        new Vector3(-5.0f, .0f, -78.0f),
        new Vector3(18.0f, .0f, -78.0f),
        new Vector3(10.0f, .0f, -90.0f),
        new Vector3(-3.0f, .0f, -125.0f),
        new Vector3(25.0f, .0f, -145.0f),
        new Vector3(-19.0f, .0f, -141.0f),
        new Vector3(1.0f, .0f, -180.0f),
        new Vector3(-18.0f, .0f, -175.0f),
        new Vector3(14.0f, .0f, -183.0f),
        new Vector3(11.0f, .0f, -222.0f),
        new Vector3(-27.0f, .0f, -198.0f)
        // 19 Spots
    };

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        curPatrolSpot = patrolSpot[Random.Range(0, 19)];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Distance Zombie-Player
        dist = Vector3.Distance(this.transform.position, target.position);

        // For Test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            agent.SetDestination(target.position);
        }

        // Tracking Mode
        if (dist < 15.0f /* Player is near a zombie == hearing distance */)
        {
            // Start Tracking Player
            agent.SetDestination(target.position);

            if (handGun.magazine.bulletNum == 0 /* Bullet Num is 0 -> Fast Run to Player */)
            {
                agent.speed = 4.0f; // Fast Run Speed
            } else if (handGun.fiered/* Listened Player Gun Shoot Sound ->  Run to Player */)
            {
                agent.speed = 3.2f; // Run Speed
            } else if (footStep.isFootSoundPlaying)
            {
                agent.speed = 2.5f; // Walk Speed
            } else
            {
                agent.speed = 2.0f; // Slow Walk Speed
            }
        }
        else // Patrol mode
        {
            agent.speed = 2.0f;
            agent.SetDestination(curPatrolSpot);
            if (Vector3.Distance(transform.position, curPatrolSpot) < 3.0f)
            {
                curPatrolSpot = patrolSpot[Random.Range(0, 19)];
            }
        }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        // Door Open / Close Event
        if (other.tag == "Door")
        {
            agent.speed = 0.5f;
            doorOpenDelay = 0.5f;
            StartCoroutine(DelayCoroutine());
        }
    } */

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Door Open / Close Event
        if (collision.gameObject.CompareTag("Door"))
        {
            Debug.Log("Opened Door");
            agent.speed = 0.5f;
            doorOpenDelay = 0.5f;
            StartCoroutine(DelayCoroutine());
        }
    }

    private IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(doorOpenDelay);
        doorOpenDelay = 0;
        agent.speed = 2.5f;
    }

}
