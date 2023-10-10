using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavTest : MonoBehaviour
{
    NavMeshAgent agent;

    [SerializeField] Transform target;

    private float doorOpenDelay;
    private float dist;

    private Vector3 curPatrolSpot;
    private Vector3[] patrolSpot =
    {
        new Vector3(8.5f, 1.0f, -20.0f),
        new Vector3(8.5f, 1.0f, -32.0f),
        new Vector3(-6.8f, 1.0f, -28.6f),
        new Vector3(-5.0f, 1.0f, -44.0f),
        new Vector3(30.0f, 1.0f, -44.0f),
        new Vector3(18.0f, 1.0f, -56.0f),
        new Vector3(30.0f, 1.0f, -94.0f),
        new Vector3(-18.0f, 1.0f, -94.0f),
        new Vector3(-5.0f, 1.0f, -78.0f),
        new Vector3(18.0f, 1.0f, -78.0f),
        new Vector3(10.0f, 1.0f, -90.0f),
        new Vector3(-3.0f, 1.0f, -125.0f),
        new Vector3(25.0f, 1.0f, -145.0f),
        new Vector3(-19.0f, 1.0f, -141.0f),
        new Vector3(1.0f, 1.0f, -180.0f),
        new Vector3(-18.0f, 1.0f, -175.0f),
        new Vector3(14.0f, 1.0f, -183.0f),
        new Vector3(11.0f, 1.0f, -222.0f),
        new Vector3(-27.0f, 1.0f, -198.0f)
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

        // Mode Change
        if (dist < 15.0f /* && Listened Player Walking Sound */)
        {
            agent.speed = 2.5f; // Walk Speed
            agent.SetDestination(target.position);
        }
        else if (dist < 15.0f /* && Listened Player Gun Shoot Sound */)
        {
            agent.speed = 4.0f; // Run Speed
            agent.SetDestination(target.position);
        }
        else // patrol mode
        {
            agent.speed = 2.5f;
            agent.SetDestination(curPatrolSpot);
            if (Vector3.Distance(transform.position, curPatrolSpot) < 1.0f)
            {
                curPatrolSpot = patrolSpot[Random.Range(0, 19)];
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Door Open / Close Event
        if (other.tag == "Door")
        {
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
