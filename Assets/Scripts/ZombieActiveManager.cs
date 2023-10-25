using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieActiveManager : MonoBehaviour
{
    [SerializeField] private GameObject firstZombie, zombie1, zombie2, zombie3;
    public bool isFirstZombieSpawnTime = false;
    public bool isZombieSpawnTime = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFirstZombieSpawnTime)
        {
            firstZombie.SetActive(true);
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

    }

    public void ActiveNormalZombie()
    {
        isZombieSpawnTime = true;
    }
}
