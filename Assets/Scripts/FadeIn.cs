using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private Image Panel;
    [SerializeField] private Material startSceneSkyboxMat;
    [SerializeField] private Material defaultSkyboxMat;
    float time = 0f;
    float F_time = 5.0f;

    private void Awake()
    {
        FadeInBlack();
    }

    private void FadeInBlack()
    {
        switch (SceneManager.sceneCount)
        {
            case 0:
                RenderSettings.skybox = startSceneSkyboxMat;
                break;
            case 1:
            case 2:
                RenderSettings.skybox = defaultSkyboxMat;
                break;
        }
        StartCoroutine(FadeInFlow());
    }

    private IEnumerator FadeInFlow()
    {
        Color alpha = Panel.color;
        alpha.a = 1;
        Panel.gameObject.SetActive(true);

        time = 0f;
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
