using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundManager;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //������ �ð� �ý��� ����
    public int curTime;//DateTime.Now.ToString(); �� ���� ���
    public int expectPlayTime; //�� ���� ���

    //�������� = Dialogue Proceed Num;
    public int curPhaseNum = 0;
    public int totalPhaseNum = 0;

    public bool[] clues;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Debug.LogError("Duplicated GameManager", gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
}
