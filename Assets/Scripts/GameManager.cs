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

    //������ �ð� �ý��� ����
    private float startTime;//DateTime.Now.ToString(); �� ���� ���
    public float expectPlayTime; //�� ���� ���

    //�������� = Dialogue Proceed Num;
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

        // ���� ��� �ð��� ǥ���ϰų� �ٸ� �۾��� Ȱ���� �� �ֽ��ϴ�.

        float percentA = currentTime / expectPlayTime * 100;
        float percentB = curPhaseNum / totalPhaseNum * 100;

        float percentdiff = percentB - percentA;

        float normalizedValue = NormalizePercentDiff(percentdiff);

        userResponsePostProcessingVolume.weight = normalizedValue;
    }

    private float NormalizePercentDiff(float percentdiff)
    {
        // percentdiff ���� 0���� 1 ������ ������ ��ȯ
        if (percentdiff <= 0) percentdiff = 0;
        else if (percentdiff >= 100) percentdiff = 100;

        // percentdiff ���� -100���� 100���� 0���� 1 ������ ������ ����
        float normalizedValue = percentdiff / 100;

        return normalizedValue;
    }
}
