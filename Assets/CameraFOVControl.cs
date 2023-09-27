using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFOVControl : MonoBehaviour
{
    public float offset = 3;

    // ������ FOV ���� �����ϱ� ���� ����
    private float originalFOV;
    private float fovChange;
    void Start()
    {
        // ���� ���� �� ������ FOV ���� �����մϴ�.
        originalFOV = Camera.main.fieldOfView;
    }

    void Update()
    {
        float fbmove = Input.GetAxis("Vertical"); // -1 to 1
        float lrmove = Input.GetAxis("Horizontal"); // -1 to 1

        // FOV�� ������ ���� ����մϴ�.
        if (fbmove >= 0)//���� or ����
        {
            fovChange = fbmove * 1f + Mathf.Abs(lrmove) * 0.5f * offset;
        }
        else//����
        {
            fovChange = fbmove * 1f * offset;
        }

        // FOV�� ������ ������ �����մϴ�. ���� FOV ������ ���氪�� ���ϰų� ���� ���ο� FOV ���� ����մϴ�.
        Camera.main.fieldOfView = Mathf.Clamp(originalFOV - fovChange, originalFOV - offset * 1.5f, originalFOV + offset * 1.5f);
    }
}