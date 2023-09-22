using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireBulletOnActivate : MonoBehaviour
{
    [Header("Prefab References")]
    public GameObject bullet;
    public GameObject cartridge;

    [Header("ParticleSystem References")]
    public ParticleSystem MuzzleFlashParticle;

    [Header("Location References")]
    public Transform bulletSpawnPoint;
    public Transform cartridgeSpawnPoint;

    public Animator shotAnime;

    [Header("Settings")]
    public float fireSpeed = 400f;
    public float cartridgeSpeed = 5f;

    [Header("Sounds")]
    public AudioSource source;
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public AudioClip reloadInSound;
    public AudioClip reloadOutSound;
    public AudioClip noAmmoSound;

    [Header("magazine")]
    public Magazine magazine;
    public XRSocketInteractor socketInteractor;

    private bool hasSlide = true;
    
    void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.activated.AddListener(FireBullet);

        socketInteractor.selectEntered.AddListener(AddMagazine);
        socketInteractor.selectExited.AddListener(RemoveMagazine);
    }

    public void FireBullet(ActivateEventArgs arg)
    {
        if (magazine && magazine.bulletNum > 0 && hasSlide)
            shotAnime.SetTrigger("doShot");
        else 
            source.PlayOneShot(noAmmoSound);
    }

    void Shoot()
    {
        magazine.bulletNum--;

        source.PlayOneShot(fireSound);

        GameObject spawnedBullet = Instantiate(bullet);

        spawnedBullet.transform.position = bulletSpawnPoint.position;
        spawnedBullet.transform.rotation = bulletSpawnPoint.rotation;

        MuzzleFlashParticle.Play();

        spawnedBullet.GetComponentInChildren<Rigidbody>().velocity = bulletSpawnPoint.forward * fireSpeed;

        Destroy(spawnedBullet, 5);
    }

    void CasingRelease()
    {
        GameObject spawnedCartridge = Instantiate(cartridge);

        spawnedCartridge.transform.position = cartridgeSpawnPoint.position;
        spawnedCartridge.transform.rotation = cartridgeSpawnPoint.rotation;

        spawnedCartridge.GetComponentInChildren<Rigidbody>().velocity = new Vector3(1, Random.Range(1f, 1.5f), Random.Range(-1.5f, -1f)) * Random.Range(0.5f, 1f);
        spawnedCartridge.GetComponentInChildren<Rigidbody>().angularVelocity = new Vector3(Random.Range(1f, 1.5f), Random.Range(1f, 1.5f), Random.Range(-1.5f, -1f));

        Destroy(spawnedCartridge, 8);
    }

    public void AddMagazine(SelectEnterEventArgs args)
    {
        magazine = args.interactableObject.transform.GetComponent<Magazine>();
        source.PlayOneShot(reloadInSound);
        hasSlide = false;
    }

    public void RemoveMagazine(SelectExitEventArgs args)
    {
        magazine = null;
        source.PlayOneShot(reloadOutSound);
    }

    public void Slide()
    {
        hasSlide = true;
        source.PlayOneShot(reloadSound);
    }
}
