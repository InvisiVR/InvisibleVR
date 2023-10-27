using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ZombieActiveManager : MonoBehaviour
{
    [SerializeField] private GameObject firstZombie, zombie1, zombie2, zombie3;
    public bool isFirstZombieSpawnTime = false;
    public bool isZombieSpawnTime = false;

    [SerializeField] private TextTrigger onChasedTrigger;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFirstZombieSpawnTime)
        {
            try
            {
                firstZombie.SetActive(true);
            } catch (MissingReferenceException) { }

            isFirstZombieSpawnTime = false;
        }

        if (isZombieSpawnTime)
        {
            zombie1.SetActive(true);
            zombie2.SetActive(true);
            zombie3.SetActive(true);
            isZombieSpawnTime = false;
        }
    }

    public void ActiveFirstZombie()
    {
        StartCoroutine(WaitAndActiveFirstZombie());
    }

    public void ActiveNormalZombie()
    {
        isZombieSpawnTime = true;
    }

    private IEnumerator WaitAndActiveFirstZombie()
    {
        yield return new WaitForSeconds(15f);
        onChasedTrigger.TriggerTriggered();
        isFirstZombieSpawnTime = true;
    }
}
