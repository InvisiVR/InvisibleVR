using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SenarioDoor : Door
{
    public XRSocketInteractor socketInteractor1;
    public XRSocketInteractor socketInteractor2;

    public bool isLocked = false;

    public int doorNum;

    private void Awake()
    {
        // 이벤트 리스너 등록
        socketInteractor1.selectEntered.AddListener(OnObjectAttached);
        socketInteractor1.selectExited.AddListener(OnObjectDetached);

        socketInteractor2.selectEntered.AddListener(OnObjectAttached);
        socketInteractor2.selectExited.AddListener(OnObjectDetached);
    }

    private void OnObjectAttached(SelectEnterEventArgs args)
    {
        if(args.interactableObject.transform.GetComponent<Key>().keyNum == doorNum) DoorUnlockSuccess();
        else DoorUnlockFailed();
    }

    private void OnObjectDetached(SelectExitEventArgs args)
    {
        Debug.Log("소켓에서 오브젝트 제거: " + args.interactableObject.transform.name);
    }

    public void DoorUnlockSuccess()
    {
        isLocked = false;
        source.PlayOneShot(doorClips[Random.Range(7, 9)]);
    }

    public void DoorUnlockFailed()
    {
        source.PlayOneShot(doorClips[Random.Range(5, 7)]);
    }

    public override void DoorOpenClose()
    {
        if (isLocked)
        {
            source.PlayOneShot(doorClips[Random.Range(2, 4)]);
        }
        else
        {
            if (isOpened)
            {
                //Close
                source.PlayOneShot(doorClips[1]);
                isOpened = false;
            }
            else
            {
                //Open
                source.PlayOneShot(doorClips[0]);
                isOpened = true;
            }
            animator.SetBool("open", isOpened);
        }
    }
}
