using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeOut : MonoBehaviour
{
    [SerializeField] private Image Panel;
    [SerializeField] private AudioSource textSound;

    [SerializeField] private int SceneNumToGo;

    float time_fade = 0f;
    float time_sound = 0f;
    [SerializeField] float F_time;

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

        while (alpha.a < 1f)
        {
            time_fade += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time_fade);
            Panel.color = alpha;

            time_sound += Time.deltaTime / F_time;
            textSound.volume = Mathf.Lerp(1, 0, time_sound);

            yield return null;
        }

        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene(SceneNumToGo);
    }
}
