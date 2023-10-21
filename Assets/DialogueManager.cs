using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using static SoundManager;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public TextMeshProUGUI dialogueTMP;

    public float delay = 1.0f;

    private List<Dictionary<string, object>> data_Dialog;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Debug.LogError("Duplicated SoundManager", gameObject);
    }

    private void Start()
    {
        data_Dialog = CSVReader.Read("Dialogues");

        //for (int i = 0; i < data_Dialog.Count; i++)
        //{
        //    print(data_Dialog[i]["ID"].ToString());
        //    print(data_Dialog[i]["Dialogue"].ToString());
        //    print(data_Dialog[i]["Chain"].ToString());
        //}
        StartCoroutine(CallDialogue(12));
    }

    IEnumerator CallDialogue(int ID)
    {
        int curID = ID;

        do
        {
            //Fade In
            //FadeUp(dialogueTMP);

            dialogueTMP.text = TMPBehavioursApplication(data_Dialog[curID]["Dialogue"].ToString(), data_Dialog[curID]["Mode"].ToString());

            yield return new WaitForSeconds(delay);

            //Fade Out
        }
        while (int.Parse(data_Dialog[curID]["Chain"].ToString()) + 1 == int.Parse(data_Dialog[++curID]["Chain"].ToString()));

        yield return null;
    }

    public string TMPBehavioursApplication(string dialog, string mode)
    {
        switch (mode)
        {
            case "pend":
                return "<pend a=2>" + dialog + "</pend>";
            case "dangle":
                return "<dangle>" + dialog + "</dangle>";
            case "shake":
                return "<shake a=2>" + dialog + "</shake>";
            default:
                return dialog;
        }
    }

    public void FadeUp(TextMeshProUGUI dialogueTMP)
    {
        Color targetColor = new Color(dialogueTMP.color.r, dialogueTMP.color.g, dialogueTMP.color.b, 1);
        dialogueTMP.color = targetColor;
    }

    public void FadeOut(TextMeshProUGUI dialogueTMP)
    {
        float duration = 1.0f; // 항상 1초로 고정
        Color startColor = dialogueTMP.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0); // Fade out to completely transparent

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            dialogueTMP.color = Color.Lerp(startColor, targetColor, t);
            elapsedTime += Time.deltaTime;
        }

        // Ensure the final color is fully transparent
        dialogueTMP.color = targetColor;
    }
}
