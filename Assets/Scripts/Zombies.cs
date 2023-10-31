using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Zombies : MonoBehaviour
{
    NavMeshAgent agent;
    private Animator anim;
    [SerializeField] private Transform target;
    [SerializeField] HandGun handGun;
    [SerializeField] FootstepsSound footStep;

    [SerializeField] private GameObject xrOrigin;
    [SerializeField] private GameObject jumpscareCam;
    [SerializeField] private GameObject bloodIMGobj;
    [SerializeField] private Image bloodIMG;
    [SerializeField] private GameObject FadeOutBlack;

    // Sounds
    [SerializeField] private GameObject jumpscareSound;
    [SerializeField] private GameObject HeartBeatSound;
    private AudioSource heartbeat;
    [SerializeField] private GameObject ZombieSound;

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
    //[SerializeField] private LayerMask layerMask = -1; // Layer Mask

    private float player_zombie_dist;
    private bool isFindPlayer;
    private bool isZombieDie;
    private bool isPlayerCatched;

    private int cur_mode = 0;
    private float hearing_dist = 7.0f;
    private float mustChase_dist = 3.0f;

    private float mode2delaytime;

    // Zombie HP
    public float hp;

    // Weights
    public float hpWeight = 1.0f;
    public float speedWeight = 1.0f;

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
        

        /* For Test Spots
        new Vector3(-8f, 0f, 8f),
        new Vector3(-8f, 0f, -8f),
        new Vector3(8f, 0f, -8f),
        new Vector3(8f, 0f, 8f) */
    };

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        heartbeat = HeartBeatSound.GetComponent<AudioSource>();
        curPatrolSpot = patrolSpot[Random.Range(0, 10)];
        agent.speed = 1.0f * speedWeight;
        hp = 10.0f * hpWeight;
        isPlayerCatched = false;
        isZombieDie = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        if (!xrOrigin.activeSelf && !isPlayerCatched) gameObject.SetActive(false);

        // Distance Zombie-Player
        player_zombie_dist = Vector3.Distance(this.transform.position, target.position);

        // HeartBeatSound Pitch
        heartbeat.pitch = 1.0f + 2.0f / player_zombie_dist;

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
        if (cur_mode == 0 && player_zombie_dist < hearing_dist)
        {
            // if Gun Fire Sound Heard -> spd = 1.6f
            if (handGun.fiered)
            {
                curPatrolSpot = target.position;
                cur_mode = 1;
                agent.speed = 1.6f * speedWeight;
            }
            // if Walking Sound Heard -> spd = 1.4f
            else if (footStep.isFootSoundPlaying)
            {
                curPatrolSpot = target.position;
                cur_mode = 1;
                agent.speed = 1.4f * speedWeight;
            }
        }

        // Raycast Hits OR In Distance --> Mode 2 (Chase Mode)
        FindingPlayerForRay();
        if (cur_mode < 2 && (isFindPlayer || player_zombie_dist < mustChase_dist))
        {
            cur_mode = 2;
            agent.speed = 1.8f * speedWeight;
            anim.SetInteger("mode", 1);

            // If the player is not visible for '3.0f' seconds --> Mode 0 (Patrol Mode)
            isFindPlayer = false;
            mode2delaytime = 3.0f;
            StartCoroutine(Mode2Coroutine());
        }
        DebugLayCastLine(); // For Laycast Debugging


        // MODE
        switch (cur_mode)
        {
            case 0: // 0:Patrol Mode
                if (!isPlayerCatched && !isZombieDie) agent.SetDestination(curPatrolSpot);
                anim.SetInteger("mode", 0);

                if (Vector3.Distance(transform.position, curPatrolSpot) < 2.0f)
                {
                    curPatrolSpot = patrolSpot[Random.Range(0, 10)];
                }
                break;
            case 1: // 1:Go to Sound
                if (!isPlayerCatched && !isZombieDie) agent.SetDestination(curPatrolSpot);
                anim.SetInteger("mode", 0);

                if (Vector3.Distance(transform.position, curPatrolSpot) < 2.0f)
                {
                    // Return to Patrol Mode
                    cur_mode = 0;
                    agent.speed = 1.0f * speedWeight;
                }
                break;
            case 2: // 2:Chase Mode
                if (!isZombieDie && !isPlayerCatched) agent.SetDestination(target.position);

                if (handGun.magazine.bulletNum == 0) agent.speed = 2.2f * speedWeight;
                else agent.speed = 1.8f * speedWeight;

                // Catch!!!
                if (!isPlayerCatched && player_zombie_dist < 1.5f && !isZombieDie)
                {
                    // Jumpscare Event Play!
                    isPlayerCatched = true;
                    StartJumpScare();
                    anim.SetInteger("mode", 4);
                    StartCoroutine(FadeOutStart());
                } //else agent.SetDestination(target.position);

                break;
        }

        // if HP < 0, Die Animation & Respawn
        if (!isZombieDie && hp < 0)
        {
            cur_mode = 999;
            hp = 999.0f;
            anim.SetInteger("mode", 3);
            StartCoroutine(ZombieDie());
        }

        // if Player Chatched, Start JumpScare
        if (isPlayerCatched)
        {
            cur_mode = 999;
            StartJumpScare();
            agent.enabled = false;
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

    private IEnumerator ZombieDie()
    {
        isZombieDie = true;
        agent.enabled = false;
        anim.SetInteger("mode", 3);
        HeartBeatSound.SetActive(false);
        ZombieSound.SetActive(false);

        yield return new WaitForSeconds(30.0f);

        // Respawn
        isZombieDie = false;
        agent.enabled = true;
        HeartBeatSound.SetActive(true);
        ZombieSound.SetActive(true);
        hp = 10.0f * hpWeight;
        transform.position = patrolSpot[0];
        cur_mode = 0;
        anim.SetInteger("mode", 0);
        agent.speed = 1.0f * speedWeight;
    }

    private void StartJumpScare()
    {
        xrOrigin.SetActive(false);
        jumpscareCam.SetActive(true);
        bloodIMGobj.SetActive(true);
        bloodIMG.color = new Color(1, 0, 0, Random.Range(0.05f, 0.15f));
        gameObject.GetComponent<NavMeshAgent>().enabled = false;

        HeartBeatSound.SetActive(false);
        ZombieSound.SetActive(false);
        jumpscareSound.SetActive(true);

        jumpscareCam.transform.position = transform.position + transform.forward*0.85f + new Vector3(0, 0.5f, 0);
        jumpscareCam.transform.eulerAngles = transform.eulerAngles + new Vector3(-65f + Random.Range(-1.5f, 1.5f), 180f + Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f));
    }

    private IEnumerator FadeOutStart()
    {
        yield return new WaitForSeconds(5.0f);
        FadeOutBlack.SetActive(true);
    }
}
