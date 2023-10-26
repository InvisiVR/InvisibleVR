using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeOut : MonoBehaviour
{
    [SerializeField] private Image Panel;

    [SerializeField] private int SceneNumToGo;
    float time = 0f;
    float F_time = 4.0f;

    private void Awake()
    {
        FadeInBlack();
    }

    private void FadeInBlack()
    {
        StartCoroutine(FadeInFlow());
    }

    private IEnumerator FadeInFlow()
    {
        Color alpha = Panel.color;
        alpha.a = 0;
        Panel.gameObject.SetActive(true);

        time = 0f;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }

        yield return new WaitForSeconds(5.0f);

        SceneManager.LoadScene(SceneNumToGo);
    }
}
