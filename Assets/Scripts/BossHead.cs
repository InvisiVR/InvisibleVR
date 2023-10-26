using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHead : MonoBehaviour
{
    public GameObject player;
    Vector3 HeadOffset = new Vector3(0, -40, -100);

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.LookAt(player.transform);
        //this.transform.rotation *= Quaternion.Euler(HeadOffset);
    }
}
