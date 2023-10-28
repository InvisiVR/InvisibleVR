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

        // ���� ��� �ð��� ǥ���ϰų� �ٸ� �۾��� Ȱ���� �� �ֽ��ϴ�.
        Debug.Log("���� �÷��� Ÿ��: " + currentTime + "�� / ���� ���� �ð� ����: " + currentTime / expectPlayTime * 100 + "%");
        Debug.Log("���� ���൵: " + curPhaseNum / totalPhaseNum * 100 + "%");

        float percentA = currentTime / expectPlayTime * 100;
        float percentB = curPhaseNum / totalPhaseNum * 100;

        float percentdiff = percentA - percentB;//�ִ� 100, �ּ� -100�� percentdiff�� ���� �ִ� 1, �ּ� 0���� ��ȯ���ִ� �Լ� �� �Ʒ��� �ۼ�����

        float normalizedValue = NormalizePercentDiff(percentdiff);

        // normalizedValue�� ����ϰų� �ٸ� �۾��� Ȱ��
        Debug.Log("Normalized Value: " + normalizedValue);
    }

    private float NormalizePercentDiff(float percentdiff)
    {
        // percentdiff ���� 0���� 1 ������ ������ ��ȯ
        if (percentdiff > 100) percentdiff = 100;
        if (percentdiff < -100) percentdiff = -100;

        // percentdiff ���� 0���� 1 ������ ������ ����
        float normalizedValue = (percentdiff + 100) / 200;

        return normalizedValue;
    }
}
