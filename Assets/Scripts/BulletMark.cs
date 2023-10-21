using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMark : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag != "HandGun")
        {
            if (collision.transform.tag == "Zombie")
            {
                Debug.Log("ZOMBIE HIT!");
                collision.gameObject.GetComponent<Zombies>().hp -= 3.5f;
            }
            gameObject.SetActive(false);

            Destroy(transform.parent.gameObject, 5f);
        }
    }
}
