using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadFollow : MonoBehaviour
{
    public GameObject player;
    Vector3 HeadOffset = new Vector3(0, -90, -100);

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.LookAt(player.transform, Vector3.up);
        this.transform.rotation *= Quaternion.Euler(HeadOffset);
    }
}
