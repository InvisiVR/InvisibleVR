using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class HandgunFlash : MonoBehaviour
{
    [Header("Lights")]
    private Light lit;

    [Header("Sounds")]
    private AudioSource source;
    public AudioClip lightOn;
    public AudioClip lightOff;

    // Start is called before the first frame update
    //void Start()
    //{
    //    lit = GameObject.Find("HandgunFlash").GetComponent<Light>();
    //    source = GetComponent<AudioSource>();
    //}

    public void LightOn()
    {
        lit.enabled = true;
        source.PlayOneShot(lightOn);
    }

    public void LightOff()
    {
        lit.enabled = false;
        source.PlayOneShot(lightOff);
    }
}
