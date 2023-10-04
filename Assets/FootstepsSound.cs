using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class FootstepsSound : MonoBehaviour
{
    public CharacterController characterController;
    public AudioClip[] walkingSound;

    private AudioSource audioSource;
    private int idx = 0;
    private bool isMoving;
    private bool isWalking;

    private InputData _inputData;

    void Start()
    {
        _inputData = GetComponent<InputData>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        MoveSfx();
    }

    void MoveSfx()
    {
        if (_inputData._leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 continuousVector))
        {
            if (Mathf.Abs(continuousVector.x) > 0.1 || Mathf.Abs(continuousVector.y) > 0.1) isMoving = true;
            else isMoving = false;
        }

        if (isMoving)
        {
            if (!audioSource.isPlaying) audioSource.Play();
            else audioSource.Stop();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        idx = 0;
        audioSource.clip = walkingSound[1];

        //if (collision.gameObject.CompareTag("¹Ù´Ú"))
        //{
        //    idx = 0;
        //    isWalking = true;
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        idx = 0;

        //if (collision.gameObject.CompareTag("¹Ù´Ú"))
        //{
        //    isWalking = false;
        //}
    }
}
