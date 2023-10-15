using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isLocked = false;
    public bool isOpened;

    [Header("Animations")]
    Animator animator;

    [Header("Sounds")]
    private AudioSource source;
    public AudioClip[] doorClips;

    void Start()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    public void DoorOpenClose()
    {
        if (isLocked)
        {
            source.PlayOneShot(doorClips[Random.Range(2, 4)]);
        }
        else
        {
            if (isOpened)
            {
                //Close
                source.PlayOneShot(doorClips[1]);
                isOpened = false;
            }
            else
            {
                //Open
                source.PlayOneShot(doorClips[0]);
                isOpened = true;
            }
            animator.SetBool("open", isOpened);
        }
    }

    public void DoorUnlock()
    {
        isLocked = false;
        source.PlayOneShot(doorClips[Random.Range(5, 7)]);
    }
}
