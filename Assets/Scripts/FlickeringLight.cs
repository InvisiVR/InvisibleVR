using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public GameObject[] FlickeringObjects;

    public Light light1;
    public Light light2;
    public float minTime;
    public float maxTime;
    public float timer;

    public AudioSource aus;
    public AudioClip lightAudio;

    public AudioClip[] lightClips;

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

    public void FlickerLightOne()
    {
        if (timer > 0) timer -= Time.deltaTime;

        if (timer < 0)
        {
           for (int i = 0; i < FlickeringObjects.Length; i++)
           {
               FlickeringObjects[i].SetActive(!FlickeringObjects[i].activeSelf);
           }
           timer = Random.Range(minTime, maxTime);
        }
    }

    public void LightOnOff()
    {
        for (int i = 0; i < FlickeringObjects.Length; i++)
        {
            aus.PlayOneShot(lightClips[Random.Range(0, 2)]);

            FlickeringObjects[i].SetActive(!FlickeringObjects[i].activeSelf);
        }
    }
}
