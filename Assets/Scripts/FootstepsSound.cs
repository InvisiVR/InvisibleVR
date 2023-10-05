using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class FootstepsSound : MonoBehaviour
{
    public AudioClip[] walkingSound;

    private AudioSource audioSource;
    private RaycastHit hit;
    private int idx = 0;
    private bool isMoving;
    private bool isGround;

    private InputData _inputData;

    void Start()
    {
        _inputData = GetComponent<InputData>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RaycastFloorCheck();
        MoveSfx();
    }

    void RaycastFloorCheck()
    {
        //Ray Check
        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            Debug.Log("hit point : " + hit.point + ", distance : " + hit.distance + ", name : " + hit.collider.name);
            Debug.DrawRay(transform.position, -transform.up * hit.distance, Color.red);

            isGround = true;
        }
        else
        {
            Debug.DrawRay(transform.position, -transform.up * 0.1f, Color.blue);

            isGround = false;
        }
    }

    void MoveSfx()
    {
        //if (_inputData._leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 continuousVector))
        //{
        //    if (Mathf.Abs(continuousVector.x) > 0.1f || Mathf.Abs(continuousVector.y) > 0.1f) isMoving = true;
        //    else isMoving = false;
        //}

        if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f) isMoving = true;
        else isMoving = false;

        if (isGround && isMoving)
        {
            Debug.Log("phase1");
            if (!audioSource.isPlaying)
            {
                Debug.Log("phase2");
                audioSource.Play();
            }
        }
        else
        {
            Debug.Log("Nope" + isMoving + " " + isGround);
            audioSource.Stop();
        }
    }
}
