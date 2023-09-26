using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpened;

    [Header("Animations")]
    Animator animator;

    [Header("Sounds")]
    private AudioSource source;
    public AudioClip doorOpen;
    public AudioClip doorClose;

    void Start()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        isOpened = animator.GetBool("open");
    }

    public void DoorOpenClose()
    {
        if (isOpened) {
            //close
            isOpened = false;
            source.PlayOneShot(doorClose);
        }
        else {
            //open
            isOpened = true;
            source.PlayOneShot(doorOpen);
        }
        animator.SetBool("open", isOpened);
    }
}
