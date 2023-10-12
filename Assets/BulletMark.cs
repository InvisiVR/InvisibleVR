using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMark : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag != "HandGun")
        {
            gameObject.SetActive(false);

            Destroy(transform.parent.gameObject, 5f);
        }
    }
}
