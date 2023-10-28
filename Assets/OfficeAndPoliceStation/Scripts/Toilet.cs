using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : MonoBehaviour
{
    public bool isOpened = false;
    public Animator animator;
    public AudioSource source;
    public AudioClip[] doorClips;

    private void Start()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }
    public void ToiletOpenClose()
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
