using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Second : MonoBehaviour
{
    public AudioSource aus;
    public AudioClip laugh;

    private void Start()
    {
        aus = GetComponent<AudioSource>();
    }
    private void OnTriggerStay(Collider other)
    {
        aus.PlayOneShot(laugh);
    }
}
