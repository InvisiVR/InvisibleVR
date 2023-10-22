using Febucci.UI;
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

    public AudioSource playerAudioSource;

    public AudioClip[] typingAudioClips;

    public float fadeInDelay = 1.0f;
    public float fadeOutDelay = 1.0f;

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
            dialogueTMP.gameObject.GetComponent<TypewriterByCharacter>().StartShowingText();

            dialogueTMP.text = TMPBehavioursApplication(data_Dialog[curID]["Dialogue"].ToString(), data_Dialog[curID]["Mode"].ToString());

            yield return new WaitForSeconds(fadeInDelay + fadeOutDelay);
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

    //public void FadeUp(TextMeshProUGUI dialogueTMP)
    //{
    //    Color targetColor = new Color(dialogueTMP.color.r, dialogueTMP.color.g, dialogueTMP.color.b, 1);
    //    dialogueTMP.color = targetColor;
    //}

    public void FadeOut()
    {
        // Ensure the final color is fully transparent
        dialogueTMP.gameObject.GetComponent<TypewriterByCharacter>().StartDisappearingText();

        float duration = fadeOutDelay;

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
        }
    }
}
