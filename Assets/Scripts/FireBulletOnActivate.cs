using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireBulletOnActivate : MonoBehaviour
{

    public GameObject bullet;
    public GameObject cartridge;
    public Transform bulletSpawnPoint;
    public Transform cartridgeSpawnPoint;

    public Animator anime;

    public float fireSpeed = 20f;
    public float cartridgeSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(FireBullet);
    }

    public void FireBullet(ActivateEventArgs arg)
    {   
        GameObject spawnedBullet = Instantiate(bullet);
        GameObject spawnedCartridge = Instantiate(cartridge);

        spawnedBullet.transform.position = bulletSpawnPoint.position;
        spawnedBullet.transform.rotation = bulletSpawnPoint.rotation;

        spawnedCartridge.transform.position = cartridgeSpawnPoint.position;
        spawnedCartridge.transform.rotation = cartridgeSpawnPoint.rotation;

        anime.SetTrigger("doShot");

        spawnedBullet.GetComponentInChildren<Rigidbody>().velocity = bulletSpawnPoint.forward * fireSpeed;
        spawnedCartridge.GetComponentInChildren<Rigidbody>().velocity = new Vector3(1, Random.Range(1f, 1.5f), Random.Range(-1.5f, -1f)) * Random.Range(0.5f, 1f);

        

        Destroy(spawnedBullet, 5);
        Destroy(spawnedCartridge, 8);
    }
}
