using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Casing : MonoBehaviour
{
    [SerializeField]
    private float destroyTime = 5.0f;
    [SerializeField]
    private float casingSpin = 1.0f;
    [SerializeField]
    private AudioClip[] audioClips;

    private Rigidbody rigidBody;
    private AudioSource audioSource;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Setup(Vector3 direction)
    {
        rigidBody.velocity = new Vector3(direction.x, 1.0f, direction.z);
        rigidBody.angularVelocity = new Vector3(
            Random.Range(-casingSpin, casingSpin),
            Random.Range(-casingSpin, casingSpin),
            Random.Range(-casingSpin, casingSpin));
    }

    private void OnCollisionEnter(Collision collision)
    {
        int index = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }
}