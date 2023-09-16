using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//#if UNITY_EDITOR
//using UnityEditor;
//#endif

public class GrabHandPose : MonoBehaviour
{
    public float poseTransitionDuration = 0.2f;

    public HandData leftHandPose;
    public HandData rightHandPose;//Ãß°¡

    private Vector3 startingHandPosition;
    private Vector3 finalHandPosition;
    private Quaternion startingHandRotation;
    private Quaternion finalHandRotation;

    private Quaternion[] startingFingerRotations;
    private Quaternion[] finalFingerRotations;

    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(SetupPose);
        grabInteractable.selectExited.AddListener(UnSetPose);

        leftHandPose.gameObject.SetActive(false);
        rightHandPose.gameObject.SetActive(false);
    }

    public void SetupPose(BaseInteractionEventArgs args)
    {
        if (args.interactorObject is XRDirectInteractor)
        {
            HandData handData = args.interactorObject.transform.GetComponentInChildren<HandData>();
            handData.animator.enabled = false;

            if (handData.handType == HandData.HandModelType.Left)
            {
                SetHandDataValues(handData, leftHandPose);
            }
            else if (handData.handType == HandData.HandModelType.Right)
            {
                SetHandDataValues(handData, rightHandPose);
            }
            
            //SetHandData(handData, finalHandPosition, finalHandRotation, finalFingerRotations);
            /*added*/
            StartCoroutine(SetHandDataRoutine(handData, finalHandPosition, finalHandRotation, finalFingerRotations, startingHandPosition, startingHandRotation, startingFingerRotations));
        }
    }

    public void UnSetPose(BaseInteractionEventArgs args)
    {
        if (args.interactorObject is XRDirectInteractor)
        {
            HandData handData = args.interactorObject.transform.GetComponentInChildren<HandData>();
            handData.animator.enabled = true;

            //SetHandData(handData, finalHandPosition, finalHandRotation, finalFingerRotations);
            /*added*/
            StartCoroutine(SetHandDataRoutine(handData, startingHandPosition, startingHandRotation, startingFingerRotations, finalHandPosition, finalHandRotation, finalFingerRotations));
        }
    }

    public void SetHandDataValues(HandData h1, HandData h2)
    {
        startingHandPosition = new Vector3(h1.root.localPosition.x / h1.root.localScale.x, h1.root.localPosition.y / h1.root.localScale.y, h1.root.localPosition.z / h1.root.localScale.z);
        finalHandPosition = new Vector3(h2.root.localPosition.x / h2.root.localScale.x, h2.root.localPosition.y / h2.root.localScale.y, h2.root.localPosition.z / h2.root.localScale.z);

        startingHandRotation = h1.root.localRotation;
        finalHandRotation = h2.root.localRotation;

        startingFingerRotations = new Quaternion[h1.fingerBones.Length];
        finalFingerRotations = new Quaternion[h2.fingerBones.Length];

        for (int i = 0; i < h1.fingerBones.Length; i++)
        {
            startingFingerRotations[i] = h1.fingerBones[i].localRotation;
            finalFingerRotations[i] = h2.fingerBones[i].localRotation;
        }
    }

    public void SetHandData(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation)
    {
        h.root.localPosition = newPosition;
        h.root.localRotation = newRotation;

        for (int i = 0; i < newBonesRotation.Length; i++)
        {
            h.fingerBones[i].localRotation = newBonesRotation[i];
        }
    }

    /*added*/
    public IEnumerator SetHandDataRoutine(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation, Vector3 startingPosition, Quaternion startingRotation, Quaternion[] startingBonesRotation)
    {
        float timer = 0;

        while (timer < poseTransitionDuration)
        {
            Vector3 p = Vector3.Lerp(startingPosition, newPosition, timer / poseTransitionDuration);
            Quaternion r = Quaternion.Lerp(startingRotation, newRotation, timer / poseTransitionDuration);

            h.root.localPosition = p;
            h.root.localRotation = r;

            for (int i = 0; i < newBonesRotation.Length; i++)
            {
                h.fingerBones[i].localRotation = Quaternion.Lerp(startingBonesRotation[i], newBonesRotation[i], timer / poseTransitionDuration);
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

//#if UNITY_EDITOR
//    [MenuItem("Tools/Mirror Selected Right Grab Pose")]
//    public static void MirrorRightPose()
//    {
//        Debug.Log("MIRROR RIGHT POSE");
//        GrabHandPose handPose = Selection.activeGameObject.GetComponent<GrabHandPose>();
//        handPose.MirrorPose(handPose.leftHandPose, handPose.rightHandPose);
//    }
//#endif

//    public void MirrorPose(HandData postToMirror, HandData poseUsedToMirror)
//    {
//        Vector3 mirroredPosition = poseUsedToMirror.root.localPosition;
//        mirroredPosition.x *= -1;

//        Quaternion mirroredQuaternion = poseUsedToMirror.root.localRotation;
//        mirroredPosition.y *= -1;
//        mirroredQuaternion.z *= -1;

//        postToMirror.root.localPosition = mirroredPosition;
//        postToMirror.root.localRotation = mirroredQuaternion;

//        for (int i = 0; i < poseUsedToMirror.fingerBones.Length; i++)
//        {
//            postToMirror.fingerBones[i].localRotation = poseUsedToMirror.fingerBones[i].localRotation;
//        }
//    }
}
