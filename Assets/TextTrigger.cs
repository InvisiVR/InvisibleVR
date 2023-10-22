using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerType { localtionTrigger, InteractionTrigger, EncounterTrigger, DodgeTrigger }

public class TextTrigger : MonoBehaviour
{
    public TriggerType triggerType;
    public int textID;
    public float delay;
    public bool isTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if(triggerType == TriggerType.localtionTrigger)
        {
            if (!isTriggered)
            {
                if (other.tag == "Player")
                {
                    StartCoroutine(TriggerDelay());

                    isTriggered = true;
                }
            }
        }
    }

    public void InteractionTriggerTriggered()
    {
        if(triggerType == TriggerType.InteractionTrigger)
        {
            if (!isTriggered)
            {
                isTriggered = true;

                StartCoroutine(TriggerDelay());
            }
        }
    }

    public IEnumerator TriggerDelay()
    {
        yield return new WaitForSeconds(delay);

        DialogueManager.instance.CallDialogue(textID);
    }
}
