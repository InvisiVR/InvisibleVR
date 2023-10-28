using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public AudioSource aus;
    public AudioClip siren;
    // Start is called before the first frame update
    void Start()
    {
        aus = GetComponent<AudioSource>();
        aus.loop = true;
        aus.clip = siren;
    }

    private void OnTriggerEnter(Collider other)
    {
        aus.Play();
    }
}
