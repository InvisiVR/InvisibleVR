using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FirstZombie : MonoBehaviour
{
    NavMeshAgent agent;
    private Animator anim;
    [SerializeField] private Transform target;
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

    private float player_zombie_dist;

    private bool isPlayerCatched;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        heartbeat = HeartBeatSound.GetComponent<AudioSource>();
        agent.speed = 0.5f;
        isPlayerCatched = false;
    }

    private void FixedUpdate()
    {
        // Distance Zombie-Player
        player_zombie_dist = Vector3.Distance(this.transform.position, target.position);

        // HeartBeatSound Pitch
        heartbeat.pitch = 1.0f + 2.0f / player_zombie_dist;
        anim.SetInteger("mode", 0);

        // Chase Player
        if (!isPlayerCatched) agent.SetDestination(target.position);

        // if Player Chatched, Start JumpScare
        if (!isPlayerCatched && player_zombie_dist < 1.5f)
        {
            // Jumpscare Event Play!
            isPlayerCatched = true;
            StartJumpScare();
            anim.SetInteger("mode", 4);
            StartCoroutine(FadeOutStart());
        }
        if (isPlayerCatched)
        {
            StartJumpScare();
            agent.enabled = false;
        }

        // if Player Go To out of range, Disable This
        if (player_zombie_dist > 15.0f) Destroy(gameObject);
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

        jumpscareCam.transform.position = transform.position + transform.forward * 0.8f + new Vector3(0, 0.6f, 0);
        jumpscareCam.transform.eulerAngles = transform.eulerAngles + new Vector3(-45f + Random.Range(-1.5f, 1.5f), 180f + Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f));
    }

    private IEnumerator FadeOutStart()
    {
        yield return new WaitForSeconds(5.0f);
        FadeOutBlack.SetActive(true);
    }
}
