using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFOVControl : MonoBehaviour
{
    public float offset = 3;

    // 원래의 FOV 값을 저장하기 위한 변수
    private float originalFOV;
    private float fovChange;
    void Start()
    {
        // 게임 시작 시 원래의 FOV 값을 저장합니다.
        originalFOV = Camera.main.fieldOfView;
    }

    void Update()
    {
        float fbmove = Input.GetAxis("Vertical"); // -1 to 1
        float lrmove = Input.GetAxis("Horizontal"); // -1 to 1

        // FOV를 조절할 양을 계산합니다.
        if (fbmove >= 0)//전진 or 정지
        {
            fovChange = fbmove * 1f + Mathf.Abs(lrmove) * 0.5f * offset;
        }
        else//후진
        {
            fovChange = fbmove * 1f * offset;
        }

        // FOV를 조절된 값으로 설정합니다. 원래 FOV 값에서 변경값을 더하거나 빼서 새로운 FOV 값을 계산합니다.
        Camera.main.fieldOfView = Mathf.Clamp(originalFOV - fovChange, originalFOV - offset * 1.5f, originalFOV + offset * 1.5f);
    }
}