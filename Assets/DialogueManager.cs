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
    public TypewriterByCharacter typer;

    public float fadeInDelay;
    public float fadeOutDelay;

    private int curDialogueID = 0;

    WaitForSeconds FadeInWaitForSeconds;
    WaitForSeconds FadeOutWaitForSeconds;

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
        FadeInWaitForSeconds = new WaitForSeconds(fadeInDelay);
        FadeOutWaitForSeconds = new WaitForSeconds(fadeOutDelay);
    }

    public void CallDialogue(int ID)
    {
        StopAllCoroutines();
        GameManager.instance.curPhaseNum++;
        StartDialogue(ID);
    }

    public void StartDialogue(int ID)
    {
        curDialogueID = ID;
        dialogueTMP.text = TMPBehavioursApplication(GameManager.instance.data_Dialog[curDialogueID]["Dialogue"].ToString(), GameManager.instance.data_Dialog[curDialogueID]["Mode"].ToString());
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

        typer.StartDisappearingText();
    }

    public IEnumerator NextDialogue()
    {
        if (int.Parse(GameManager.instance.data_Dialog[curDialogueID]["Chain"].ToString()) + 1 == int.Parse(GameManager.instance.data_Dialog[curDialogueID+1]["Chain"].ToString()))
        {
            yield return FadeOutWaitForSeconds;

            StartDialogue(curDialogueID + 1);
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
                return "<wiggle a=2>" + dialog + "</wiggle>";
            case "bounce":
                return "<bounce>" + dialog + "</bounce>";
            case "swing":
                return "<swing>" + dialog + "</swing>";
            case "incr":
                return "<incr>" + dialog + "</incr>";
            case "":
            default:
                return dialog;
        }
    }
}
