using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableTwoAttach : XRGrabInteractable
{
    public Transform leftAttachTransform;
    public Transform rightAttachTransform;

    public string curGrabbedHand = "None";

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
            curGrabbedHand = "Left";
        }
        else if(args.interactorObject.transform.CompareTag("Right Hand"))
        {
            attachTransform = rightAttachTransform;
            curGrabbedHand = "Right";
        }
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        for (int i = 0; i < switchOnOffObject.Length; i++)
        {
            switchOnOffObject[i].layer = LayerMask.NameToLayer("Interactable");
        }
        curGrabbedHand = "None";

        base.OnSelectExited(args);
    }
}
