using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    public bool[] isLocked = { false, false };
    public bool[] isOpened = { false, false };

    [Header("Animations")]
    Animator animator;

    [Header("Sounds")]
    private AudioSource source;
    public AudioClip[] drawerClips;

    void Start()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    public void DrawerOpenClose(int num)
    {
        if (isLocked[num])
        {
            source.PlayOneShot(drawerClips[Random.Range(2, 4)]);
        }
        else
        {
            if (isOpened[num])
            {
                //Close
                source.PlayOneShot(drawerClips[1]);
                isOpened[num] = false;
            }
            else
            {
                //Open
                source.PlayOneShot(drawerClips[0]);
                isOpened[num] = true;
            }
            animator.SetBool("isDrawerOpened" + num, isOpened[num]);
        }
    }

    public void DrawerUnlock(int num)
    {
        isLocked[num] = false;
        source.PlayOneShot(drawerClips[Random.Range(5, 7)]);
    }
}
