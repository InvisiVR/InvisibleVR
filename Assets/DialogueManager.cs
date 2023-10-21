using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SoundManager;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public TextMeshProUGUI dialogueTMP;

    public float delay = 1f;

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

        CallDialogue(15);

        StartCoroutine(CallDialogue(0));
    }

    //public void CallDialogue(int ID)
    //{
    //    int curID = ID;

    //    do
    //    {
    //        Debug.Log(data_Dialog[curID]["Dialogue"].ToString());
    //    }
    //    while (int.Parse(data_Dialog[curID]["Chain"].ToString()) + 1 == int.Parse(data_Dialog[++curID]["Chain"].ToString()));
    //}

    IEnumerator CallDialogue(int ID)
    {
        int curID = ID;

        do
        {
            //Fade In
            dialogueTMP.text = data_Dialog[curID]["Dialogue"].ToString();

            yield return new WaitForSeconds(1.5f);

            //Fade Out
        }
        while (int.Parse(data_Dialog[curID]["Chain"].ToString()) + 1 == int.Parse(data_Dialog[++curID]["Chain"].ToString()));

        yield return null;
    }
}
