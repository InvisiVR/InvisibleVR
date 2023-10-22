using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundManager;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //반응형 시간 시스템 정보
    public int curTime;//DateTime.Now.ToString(); 초 단위 계산
    public int expectPlayTime; //초 단위 계산

    //진행정보 = Dialogue Proceed Num;
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
