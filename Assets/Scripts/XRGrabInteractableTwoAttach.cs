using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableTwoAttach : XRGrabInteractable
{
    public Transform leftAttachTransform;
    public Transform rightAttachTransform;

    public GameObject[] switchOnOffObject;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        for (int i = 0; i < switchOnOffObject.Length; i++)
        {
            switchOnOffObject[i].layer = LayerMask.NameToLayer("Disinteractable");
        }

        if (args.interactorObject.transform.CompareTag("Left Hand"))
        {
            attachTransform = leftAttachTransform;
        }
        else if(args.interactorObject.transform.CompareTag("Right Hand"))
        {
            attachTransform = rightAttachTransform;
        }
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        for (int i = 0; i < switchOnOffObject.Length; i++)
        {
            switchOnOffObject[i].layer = LayerMask.NameToLayer("Interactable");
        }

        base.OnSelectExited(args);
    }
}
