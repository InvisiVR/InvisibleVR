using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class First : MonoBehaviour
{
    public Animator animator;
    private int count = 0;

    public GameObject throw_object;
    public AudioSource aus;
    public AudioClip paper;

    private void Start()
    {
        aus = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && count==0)
        {
            animator.SetTrigger("throw");
            throw_object.SetActive(true);
            throw_object.GetComponent<Rigidbody>().AddRelativeForce(Vector3.right * 250
                * Time.deltaTime, ForceMode.Impulse);
            aus.clip = paper;
            aus.PlayDelayed(0.5f);
            count++;
        }
    }
}
