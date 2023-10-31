using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SenarioDoor : Door
{
    public XRSocketInteractor socketInteractor1;
    public XRSocketInteractor socketInteractor2;

    public bool isLocked = false;
    public bool isCabinetA = false;
    public bool isLocker = false;

    public int doorNum;
    private bool existInteractionObject = false;

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
        source.PlayOneShot(doorClips[Random.Range(5, 7)]);
        existInteractionObject = true;
        Debug.Log("Unlock Success");
    }

    public void DoorUnlockFailed()
    {
        source.PlayOneShot(doorClips[Random.Range(2, 4)]);
        Debug.Log("Unlock Failed");
    }

    public void LockerOpenClose()
    {
        animator.SetBool("isLocker", isLocker);
        if (isLocked)
        {
            source.PlayOneShot(doorClips[Random.Range(2, 4)]);
        }
        else
        {
            if (A)
            {
                animator.SetBool("A", A);
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
                    if (existInteractionObject)
                    {
                        SetInteractionObject();
                    }
                }
                animator.SetBool("open", isOpened);
            }
            else if (B)
            {
                animator.SetBool("B", B);
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
                    if (existInteractionObject)
                    {
                        SetInteractionObject();
                    }
                }
                animator.SetBool("open", isOpened);
            }
            else if (C)
            {
                animator.SetBool("C", C);
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
                    if (existInteractionObject)
                    {
                        SetInteractionObject();
                    }
                }
                animator.SetBool("open", isOpened);
            }
            else if (D)
            {
                animator.SetBool("D", D);
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
                    if (existInteractionObject)
                    {
                        SetInteractionObject();
                    }
                }
                animator.SetBool("open", isOpened);
            }
        }
    }

    public override void DoorOpenClose()
    {
        animator.SetBool("isLocker", isLocker);
        animator.SetBool("isCabinetA", isCabinetA);
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
                if (existInteractionObject)
                {
                    SetInteractionObject();
                }
            }
            animator.SetBool("open", isOpened);
        }
    }

    public override void DoorOpenCloseLR()
    {
        animator.SetBool("isLocker", isLocker);
        animator.SetBool("isCabinetA", isCabinetA);
        if (isLocked)
        {
            source.PlayOneShot(doorClips[Random.Range(2, 4)]);
        }
        else
        {
            if (isLeft)
            {
                animator.SetBool("isLeft", isLeft);
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
                    if (existInteractionObject)
                    {
                        SetInteractionObject();
                    }
                }
            }
            else
            {
                animator.SetBool("isLeft", isLeft);
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
                    if (existInteractionObject)
                    {
                        SetInteractionObject();
                    }
                }
            }

            animator.SetBool("open", isOpened);
        }  
    }

    [System.Obsolete]
    public void SetInteractionObject()
    {
        if(this.gameObject == GameObject.FindWithTag("GunCabinet"))
        {
            GameObject.Find("InteractionObjects").transform.FindChild("handgun").gameObject.SetActive(true);
            GameObject.Find("InteractionObjects").transform.FindChild("PM-40_Magazine One Attach").gameObject.SetActive(true);
        }
        if (this.gameObject == GameObject.Find("Locker_door_B_Key"))
        {
            GameObject.Find("InteractionObjects").transform.FindChild("Key4").gameObject.SetActive(true);
        }
    }
}
