using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private Image Panel;
    float time = 0f;
    float F_time = 3.0f;

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
        alpha.a = 1;
        Panel.gameObject.SetActive(true);

        time = 0f;

        yield return new WaitForSeconds(1.0f);
        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }
        Panel.gameObject.SetActive(false);
    }
}
