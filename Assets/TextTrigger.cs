using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    public int TextID;

    bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            if (other.tag == "Player")
            {
                DialogueManager.instance.CallDialogue(TextID);
                isTriggered = true;
            }
        }
    }
}
