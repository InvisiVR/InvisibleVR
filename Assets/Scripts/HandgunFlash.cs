using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static Unity.VisualScripting.Member;

public class HandgunFlash : MonoBehaviour
{
    [Header("Lights")]
    public Light lit;

    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabbable = GetComponent<XRGrabInteractable>();
        grabbable.selectEntered.AddListener(LightOn);
        grabbable.selectExited.AddListener(LightOff);
    }

    public void LightOn(SelectEnterEventArgs args)
    {
        lit.enabled = true;
    }

    public void LightOff(SelectExitEventArgs args)
    {
        lit.enabled = false;
    }
}
