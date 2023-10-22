using Febucci.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using static SoundManager;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public TextMeshProUGUI dialogueTMP;

    public float fadeInDelay;
    public float fadeOutDelay;

    private int curDialogueID;

    WaitForSeconds FadeInWaitForSeconds;
    WaitForSeconds FadeOutWaitForSeconds;

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

        FadeInWaitForSeconds = new WaitForSeconds(fadeInDelay);
        FadeOutWaitForSeconds = new WaitForSeconds(fadeOutDelay);

        //for (int i = 0; i < data_Dialog.Count; i++)
        //{
        //    print(data_Dialog[i]["ID"].ToString());
        //    print(data_Dialog[i]["Dialogue"].ToString());
        //    print(data_Dialog[i]["Chain"].ToString());
        //}

        CallDialogue(12);
    }

    public void CallDialogue(int ID)
    {
        curDialogueID = ID;

        dialogueTMP.text = TMPBehavioursApplication(data_Dialog[curDialogueID]["Dialogue"].ToString(), data_Dialog[curDialogueID]["Mode"].ToString());
    }

    public void CallNextDialogue()
    {
        StartCoroutine(NextDialogue());
    }

    public void CallDisappearDialogue()
    {
        StartCoroutine(DisappearingDialogue());
    }

    public IEnumerator DisappearingDialogue()
    {
        yield return FadeInWaitForSeconds;

        dialogueTMP.gameObject.GetComponent<TypewriterByCharacter>().StartDisappearingText();
    }

    public IEnumerator NextDialogue()
    {
        if (int.Parse(data_Dialog[curDialogueID]["Chain"].ToString()) + 1 == int.Parse(data_Dialog[curDialogueID+1]["Chain"].ToString()))
        {
            yield return FadeOutWaitForSeconds;

            CallDialogue(curDialogueID + 1);
        }
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
                return "<shake a=5>" + dialog + "</shake>";
            case "wiggle":
                return "<wiggle a=2 f=5>" + dialog + "</wiggle>";
            default:
                return dialog;
        }
    }
}
