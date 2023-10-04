using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.UIElements;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(InputData))]
public class CameraFOVControl : MonoBehaviour
{

    // ������ FOV ���� �����ϱ� ���� ����
    public float offset;

    private Vector3 CameraLocalPos;//camera transform
    private float ctoffset;//camera transform offset

    private InputData _inputData;

    private float fbmove = 0;
    private float lrmove = 0;

    void Start()
    {
        // ���� ���� �� ������ FOV ���� �����մϴ�.
        _inputData = GetComponent<InputData>();
    }

    void Update()
    {
        if (_inputData._leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 continuousVector))
        {
            fbmove = continuousVector.y;//-1 to 1
            lrmove = continuousVector.x;//-1 to 1
        }

        // FOV�� ������ ���� ����մϴ�.
        if (fbmove >= 0)//����
        {
            ctoffset = (fbmove * 1f + Mathf.Abs(lrmove) * 0.5f) * offset;//-1.5 to 1.5
        }
        else//����
        {
            ctoffset = (fbmove * 1f) * offset;//-1 to 1
        }

        //���� ī�޶� ������Ʈ�� front
        Quaternion cf = Camera.main.transform.localRotation;

        Camera.main.transform.localPosition = new Vector3(CameraLocalPos.x, CameraLocalPos.y, CameraLocalPos.z + Mathf.Clamp(ctoffset, -(offset * 1.5f), offset * 1.5f));
    }
}