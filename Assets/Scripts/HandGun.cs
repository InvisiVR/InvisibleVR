using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
public class ImpactInfo
{
    public MaterialType.MaterialTypeEnum MaterialType;
    public GameObject ImpactEffect;
}

public class HandGun : MonoBehaviour
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

    [Header("Magazine")]
    public Magazine magazine;
    public XRSocketInteractor socketInteractor;

    [Header("Slide")]
    public bool hasSlide = true;

    [Header("Recoil")]

    private Rigidbody recoilBody = null;
    private Rigidbody recoilBodyOfHand = null;

    public Rigidbody leftRecoilBody;
    public Rigidbody rightRecoilBody;

    public Rigidbody leftRecoilBodyOfHand;
    public Rigidbody rightRecoilBodyOfHand;

    [Header("Effects")]
    public ImpactInfo[] ImpactElemets = new ImpactInfo[0];
    
    public float BulletDistance = 100;
    public GameObject ImpactEffect;

    public bool fiered = false;
    private float fireSoundDelay;

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
        {
            shotAnime.SetTrigger("doShot");
        }
        else if(magazine && magazine.bulletNum > 0 && !hasSlide)
        {
            shotAnime.SetBool("NoAmmo", false);
            shotAnime.SetTrigger("doReload");
            Slide();
        }
        else
        {
            if (shotAnime.GetBool("NoAmmo") == false)
            {
                shotAnime.SetBool("NoAmmo", true);
                shotAnime.SetTrigger("doNoAmmo");
            }
            source.PlayOneShot(noAmmoSound);
        }   
    }

    void Shoot()
    {
        BulletFire();
        MuzzleFlash();
        Recoil();
    }

    void Recoil()
    {
        string CHS = gameObject.GetComponent<XRGrabInteractableTwoAttach>().curGrabbedHand;

        if (CHS == "Left")
        {
            recoilBody = leftRecoilBody;
            recoilBodyOfHand = leftRecoilBodyOfHand;
        }
        else if(CHS == "Right")
        {
            recoilBody = rightRecoilBody;
            recoilBodyOfHand = rightRecoilBodyOfHand;
        }
        else if(CHS == "None")
        {
            recoilBody = null;
            recoilBodyOfHand = null;
        }

        if(recoilBody != null)
        {
            float force = 1.5f;

            recoilBody.AddForce(-transform.forward * force, ForceMode.Impulse);
            recoilBody.transform.localRotation = Quaternion.AngleAxis(-15 * force, Vector3.right);

            if (recoilBodyOfHand != null)
            {
                force *= 0.1f;
                recoilBodyOfHand.AddForce(-transform.forward, ForceMode.Impulse);
            }
            else
            {
                recoilBody.transform.localRotation = Quaternion.AngleAxis(-500 * force, Vector3.right);
            }
        }
    }

    void MuzzleFlash()
    {
        //Gun Light Muzzle Flashes
        MuzzleFlashParticle.Play();

        //Gun Fire Muzzle Flashes
        var impactEffectIstance = Instantiate(ImpactEffect, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation) as GameObject;

        Destroy(impactEffectIstance, 4);

        //Bullet Mark 
        RaycastHit hit;

        var ray = new Ray(bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.forward);
        if (Physics.Raycast(ray, out hit, BulletDistance))
        {
            Debug.Log(hit.transform.GetComponent<MeshRenderer>().material);
            Debug.Log(hit.transform.name);
            var effect = GetImpactEffect(hit.transform.gameObject);

            if (effect == null) return;

            var effectIstance = Instantiate(effect, hit.point, new Quaternion()) as GameObject;
            effectIstance.transform.LookAt(hit.point + hit.normal);

            Destroy(effectIstance, 10);
        }
    }

    void BulletFire()
    {
        magazine.bulletNum--;

        GameObject spawnedBullet = Instantiate(bullet);

        spawnedBullet.transform.position = bulletSpawnPoint.position;
        spawnedBullet.transform.rotation = bulletSpawnPoint.rotation;

        spawnedBullet.GetComponentInChildren<Rigidbody>().velocity = bulletSpawnPoint.forward * fireSpeed;

        source.PlayOneShot(fireSound);
        fiered = true;
        fireSoundDelay = 2.0f;
        StartCoroutine(FireSoundDelayCoroutine());

        if(spawnedBullet != null) Destroy(spawnedBullet, 5);
    }

    private IEnumerator FireSoundDelayCoroutine()
    {
        yield return new WaitForSeconds(fireSoundDelay);
        fireSoundDelay = 0;
        fiered = false;
    }

    void CasingRelease()
    {
        GameObject spawnedCartridge = Instantiate(cartridge);

        spawnedCartridge.transform.position = cartridgeSpawnPoint.position;
        spawnedCartridge.transform.rotation = cartridgeSpawnPoint.rotation;

        Casing casing = spawnedCartridge.GetComponentInChildren<Casing>();

        casing.Setup(transform.right);
    }

    public void AddMagazine(SelectEnterEventArgs args)
    {
        args.interactableObject.transform.gameObject.layer = LayerMask.NameToLayer("Disinteractable");
        magazine = args.interactableObject.transform.GetComponent<Magazine>();
        source.PlayOneShot(reloadInSound);
        hasSlide = false;
    }

    public void RemoveMagazine(SelectExitEventArgs args)
    {
        args.interactableObject.transform.gameObject.layer = LayerMask.NameToLayer("Interactable");
        magazine = null;
        source.PlayOneShot(reloadOutSound);
    }

    public void Slide()
    {
        hasSlide = true;
        source.PlayOneShot(reloadSound);
    }

    GameObject GetImpactEffect(GameObject impactedGameObject)
    {
        var materialType = impactedGameObject.GetComponent<MaterialType>();
        if (materialType == null) return null;
        foreach (var impactInfo in ImpactElemets)
        {
            if (impactInfo.MaterialType == materialType.TypeOfMaterial)
                return impactInfo.ImpactEffect;
        }
        return null;
    }
}
