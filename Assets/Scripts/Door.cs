using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Door : MonoBehaviour
{
    public bool isLeft;
    public bool isOpened;

    [Header("Animations")]
    protected Animator animator;

    [Header("Sounds")]
    protected AudioSource source;

    public AudioClip[] doorClips;

    void Start()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    public virtual void DoorOpenCloseLR()
    {
        if (isLeft)
        {
            animator.SetBool("isLeft", isLeft);
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
        }
        else
        {
            animator.SetBool("isLeft", isLeft);
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
        }
        
        animator.SetBool("open", isOpened);
    }

    public virtual void DoorOpenClose()
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
