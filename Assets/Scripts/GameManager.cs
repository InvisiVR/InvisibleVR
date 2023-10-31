using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

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

    public Volume userResponsePostProcessingVolume;

    [HideInInspector]
    public List<Dictionary<string, object>> data_Dialog;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "IntroScene") curPhaseNum = 0f;
    }

    private void Update()
    {
        float currentTime = Time.time - startTime;

        // 현재 경과 시간을 표시하거나 다른 작업에 활용할 수 있습니다.

        float percentA = currentTime / expectPlayTime * 100;
        float percentB = curPhaseNum / totalPhaseNum * 100;

        float percentdiff = percentB - percentA;

        float normalizedValue = NormalizePercentDiff(percentdiff);

        userResponsePostProcessingVolume.weight = normalizedValue;
    }

    private float NormalizePercentDiff(float percentdiff)
    {
        // percentdiff 값을 0에서 1 사이의 범위로 변환
        if (percentdiff <= 0) percentdiff = 0;
        else if (percentdiff >= 100) percentdiff = 100;

        // percentdiff 값을 -100에서 100에서 0에서 1 사이의 범위로 매핑
        float normalizedValue = percentdiff / 100;

        return normalizedValue;
    }
}
