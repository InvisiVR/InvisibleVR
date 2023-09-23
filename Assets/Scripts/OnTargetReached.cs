using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class OnTargetReached : MonoBehaviour
{
    public float threshold = 0.02f;
    public Transform target;
    public UnityEvent OnReached;
    public bool wasReached = false;
    private void FixedUpdate()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if(distance < threshold && !wasReached)
        {
            //Reached the target;
            OnReached.Invoke();
            wasReached = true;
        }
        else if(distance >= threshold)
        {
            wasReached = false;
        }
    }
}
