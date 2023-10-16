using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombies : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] private Transform target;

    // Raycast
    private float ray_dist = 10.0f; // Raycast Distance
    private Ray[] rays = {
        new Ray(),
        new Ray(),
        new Ray(),
        new Ray(),
        new Ray(),
        new Ray(),
        new Ray()
    };
    private RaycastHit hit1, hit2, hit3, hit4, hit5, hit6, hit7; // Racast Hits
    private Vector3 layPos;
    [SerializeField] private LayerMask layerMask = -1; // Layer Mask

    private float player_zombie_dist;
    private bool isFindPlayer;

    private int cur_mode = 0;
    private float hearing_dist = 7.0f;
    private float mustChase_dist = 3.0f;
    private float spd = 2.0f;

    private float mode2delaytime;

    private Vector3 curPatrolSpot;
    private Vector3[] patrolSpot =
    {
        // 3F Spots
        new Vector3(-18f, 9f, 26f),

        // 2F Spots
        new Vector3(-19f, 5f, 23f),
        new Vector3(1f, 5f, 24f),
        new Vector3(-8f, 5f, 30f),

        // 1F Spots
        new Vector3(-19f, 1f, 23f),
        new Vector3(-12f, 1f, 37f),
        new Vector3(28f, 1f, 31f),
        new Vector3(5f, 1f, 23f),
        new Vector3(-8f, 1f, 11f),

        // B1 Spots
        new Vector3(-7f, -3f, 26f),
        
        // --> Total 10 Spots
    };

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        curPatrolSpot = patrolSpot[Random.Range(0, 10)];
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        // Distance Zombie-Player
        player_zombie_dist = Vector3.Distance(this.transform.position, target.position);

        // Setting Raycast
        layPos = transform.position + new Vector3(0, 1.3f, 0);
        rays[0] = new Ray(layPos, (transform.forward + transform.right * -0.3f).normalized);
        rays[1] = new Ray(layPos, (transform.forward + transform.right * -0.2f).normalized);
        rays[2] = new Ray(layPos, (transform.forward + transform.right * -0.1f).normalized);
        rays[3] = new Ray(layPos, transform.forward);
        rays[4] = new Ray(layPos, (transform.forward + transform.right * 0.1f).normalized);
        rays[5] = new Ray(layPos, (transform.forward + transform.right * 0.2f).normalized);
        rays[6] = new Ray(layPos, (transform.forward + transform.right * 0.3f).normalized);

        // Heard Player's Sound --> Mode 1 (Go to place of sound)
        // ********************************************
        if (player_zombie_dist < hearing_dist)
        {
            // if Walking Sound Heard -> spd = 2.4f, Walking Ani

            // if Gun Fire Sound Heard -> spd = 2.8f, Running Ani
        }


        // Raycast Hits OR In Distance --> Mode 2 (Chase Mode)
        FindingPlayerForRay();
        if (player_zombie_dist < mustChase_dist || isFindPlayer)
        {
            cur_mode = 2;
            isFindPlayer = false;
            // If the player is not visible for '5.0f' seconds --> Mode 0 (Patrol Mode)
            mode2delaytime = 5.0f;
            StartCoroutine(Mode2Coroutine());
        }
        DebugLayCastLine(); // For Laycast Debugging


        // MODE
        switch (cur_mode)
        {
            case 0: // 0:Patrol Mode
                agent.SetDestination(curPatrolSpot);
                if (Vector3.Distance(transform.position, curPatrolSpot) < 1.0f)
                {
                    curPatrolSpot = patrolSpot[Random.Range(0, 10)];
                }
                break;
            case 1: // 1:Go to Sound
                agent.SetDestination(target.position);
                if (Vector3.Distance(transform.position, curPatrolSpot) < 1.0f)
                {
                    cur_mode = 0;
                }
                break;
            case 2: // 2:Chase Mode
                agent.SetDestination(target.position);

                // Catch!!!
                if (Vector3.Distance(transform.position, target.position) < 0.7f)
                {
                    // Jumpscare Event Play!
                    Debug.Log(this.name + " Catched!");
                }
                break;
        }
                
    }

    private void FindingPlayerForRay()
    {
        if (Physics.Raycast(rays[0], out hit1, ray_dist) && hit1.collider.gameObject.tag == "Player") isFindPlayer = true;
        if (Physics.Raycast(rays[1], out hit2, ray_dist) && hit2.collider.gameObject.tag == "Player") isFindPlayer = true;
        if (Physics.Raycast(rays[2], out hit3, ray_dist) && hit3.collider.gameObject.tag == "Player") isFindPlayer = true;
        if (Physics.Raycast(rays[3], out hit4, ray_dist) && hit4.collider.gameObject.tag == "Player") isFindPlayer = true;
        if (Physics.Raycast(rays[4], out hit5, ray_dist) && hit5.collider.gameObject.tag == "Player") isFindPlayer = true;
        if (Physics.Raycast(rays[5], out hit6, ray_dist) && hit6.collider.gameObject.tag == "Player") isFindPlayer = true;
        if (Physics.Raycast(rays[6], out hit7, ray_dist) && hit7.collider.gameObject.tag == "Player") isFindPlayer = true;
    }

    private void DebugLayCastLine()
    {
        Color cur_color;
        if (cur_mode == 2) cur_color = Color.red;
        else cur_color = Color.green;

        Debug.DrawLine(layPos, layPos + (transform.forward + transform.right * -0.3f).normalized * hit1.distance, cur_color);
        Debug.DrawLine(layPos, layPos + (transform.forward + transform.right * -0.2f).normalized * hit2.distance, cur_color);
        Debug.DrawLine(layPos, layPos + (transform.forward + transform.right * -0.1f).normalized * hit3.distance, cur_color);
        Debug.DrawLine(layPos, layPos + transform.forward * hit4.distance, cur_color);
        Debug.DrawLine(layPos, layPos + (transform.forward + transform.right * 0.1f).normalized * hit5.distance, cur_color);
        Debug.DrawLine(layPos, layPos + (transform.forward + transform.right * 0.2f).normalized * hit6.distance, cur_color);
        Debug.DrawLine(layPos, layPos + (transform.forward + transform.right * 0.3f).normalized * hit7.distance, cur_color);
    }

    private IEnumerator Mode2Coroutine()
    {
        yield return new WaitForSeconds(mode2delaytime);
        mode2delaytime = 0;
        cur_mode = 0;
    }
}
