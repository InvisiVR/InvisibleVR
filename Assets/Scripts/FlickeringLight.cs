using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light light1;
    public Light light2;
    public float minTime;
    public float maxTime;
    public float timer;

    public AudioSource aus;
    public AudioClip lightAudio;
    // Start is called before the first frame update
    void Start()
    {
        timer = Random.Range(minTime, maxTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FlickerLight();
        }
    }

    void FlickerLight()
    {
        light1.GetComponent<Light>().color = Color.red;
        light2.GetComponent<Light>().color = Color.red;

        if (timer > 0)
            timer -= Time.deltaTime;
        if (timer < 0)
        {
            light1.enabled = !light1.enabled;
            light2.enabled = !light2.enabled;
            timer = Random.Range(minTime, maxTime);
            aus.PlayOneShot(lightAudio);
        }
    }
}
