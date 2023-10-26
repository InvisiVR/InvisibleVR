using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMovement : MonoBehaviour
{
    bool isTriggered = false;

    [SerializeField] private GameObject FadeOutBlack;

    private void OnCollisionEnter(Collision collision)
    {
        SceneMovementStart();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (!isTriggered)
            {
                isTriggered = true;
                StartCoroutine(FadeOutStart());
            }
        }
    }

    public void SceneMovementStart()
    {
        if (!isTriggered)
        {
            isTriggered = true;
            StartCoroutine(FadeOutStart());
        }
        
    }

    public void GameExit()
    {
        Application.Quit();
    }

    private IEnumerator FadeOutStart()
    {
        FadeOutBlack.SetActive(true);
        yield return new WaitForSeconds(5.0f);
    }
}
