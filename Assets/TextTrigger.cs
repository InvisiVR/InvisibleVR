using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerType { localtionTrigger, InteractionTrigger, OnChasedTrigger, OffChasedTrigger }

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

    public void TriggerTriggered()
    {
        if(triggerType == TriggerType.InteractionTrigger || triggerType == TriggerType.OnChasedTrigger || triggerType == TriggerType.OffChasedTrigger)
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
