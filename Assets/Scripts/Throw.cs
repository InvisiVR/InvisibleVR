using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    private int count = 0;

    public GameObject throw_object;
    public GameObject throw_object1;
    public AudioSource aus;
    public AudioClip paper;

    private void Start()
    {
        aus = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && count == 0)
        {
            throw_object.SetActive(true);
            throw_object.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 400
                * Time.deltaTime, ForceMode.Impulse);
            throw_object1.SetActive(true);
            throw_object1.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 350
                * Time.deltaTime, ForceMode.Impulse);
            aus.clip = paper;
            aus.PlayDelayed(0.5f);
            count++;
        }
    }
}
