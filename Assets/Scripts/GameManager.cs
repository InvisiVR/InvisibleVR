using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundManager;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    //������ �ð� �ý��� ����
    private float startTime;//DateTime.Now.ToString(); �� ���� ���
    public float expectPlayTime; //�� ���� ���

    //�������� = Dialogue Proceed Num;
    public int curPhaseNum = 0;
    public int totalPhaseNum = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(instance);
        }
        DontDestroyOnLoad(instance);
    }

    void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        float currentTime = Time.time - startTime;

        // ���� ��� �ð��� ǥ���ϰų� �ٸ� �۾��� Ȱ���� �� �ֽ��ϴ�.
        Debug.Log("���� �÷��� Ÿ��: " + currentTime + "�� / ���� ���� �ð� ����: " + currentTime / expectPlayTime * 100 + "%");
        Debug.Log("���� ���൵: " + curPhaseNum / totalPhaseNum * 100 + "%");
    }
}
