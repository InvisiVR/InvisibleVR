using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieCrwalTrigger1 : MonoBehaviour
{
    [SerializeField] private GameObject crwalZombie;
    private bool isEntered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isEntered)
        {
            isEntered = true;
            StartCoroutine(StartCrwalZombieTrigger());
        }
    }

    private IEnumerator StartCrwalZombieTrigger()
    {
        crwalZombie.SetActive(true);

        yield return new WaitForSeconds(7.0f);
        Destroy(crwalZombie.gameObject);
        Destroy(gameObject);
    }
}
