using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundManager;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    //반응형 시간 시스템 정보
    private float startTime;//DateTime.Now.ToString(); 초 단위 계산
    public float expectPlayTime; //초 단위 계산

    //진행정보 = Dialogue Proceed Num;
    public int curPhaseNum = 0;
    public int totalPhaseNum = 0;

    [HideInInspector]
    public List<Dictionary<string, object>> data_Dialog;

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

    private void Start()
    {
        startTime = Time.time;

        data_Dialog = CSVReader.Read("Dialogues");

        for (int i = 0; i < data_Dialog.Count; i++)
        {
            if (data_Dialog[i]["Chain"].ToString() == "0")
            {
                totalPhaseNum++;
            }
        }
    }

    private void Update()
    {
        float currentTime = Time.time - startTime;

        // 현재 경과 시간을 표시하거나 다른 작업에 활용할 수 있습니다.
        Debug.Log("현재 플레이 타임: " + currentTime + "초 / 게임 진행 시간 비율: " + currentTime / expectPlayTime * 100 + "%");
        Debug.Log("게임 진행도: " + curPhaseNum / totalPhaseNum * 100 + "%");
    }
}
