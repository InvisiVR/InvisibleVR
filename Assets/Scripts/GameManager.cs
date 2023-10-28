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
    public float curPhaseNum = 0f;
    public float totalPhaseNum = 0f;

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

        float percentA = currentTime / expectPlayTime * 100;
        float percentB = curPhaseNum / totalPhaseNum * 100;

        float percentdiff = percentA - percentB;//최대 100, 최소 -100인 percentdiff의 값을 최대 1, 최소 0으로 변환해주는 함수 이 아래에 작성해줘

        float normalizedValue = NormalizePercentDiff(percentdiff);

        // normalizedValue를 출력하거나 다른 작업에 활용
        Debug.Log("Normalized Value: " + normalizedValue);
    }

    private float NormalizePercentDiff(float percentdiff)
    {
        // percentdiff 값을 0에서 1 사이의 범위로 변환
        if (percentdiff > 100) percentdiff = 100;
        if (percentdiff < -100) percentdiff = -100;

        // percentdiff 값을 0에서 1 사이의 범위로 매핑
        float normalizedValue = (percentdiff + 100) / 200;

        return normalizedValue;
    }
}
