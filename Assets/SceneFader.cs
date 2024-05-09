using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance { get; private set; }

    [SerializeField] private Image faderImage;
    [SerializeField] private float fadeDuration = .5f;

    void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene ( string sceneName )
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine ( string sceneName )
    {
        if (sceneName == null) yield return null;

        yield return StartCoroutine(FadeIn());
        SceneManager.LoadScene(sceneName);
        yield return StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn ()
    {
        float counter = 0f;
        Color originalColor = faderImage.color;  // Store the original color
        while (counter < fadeDuration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, counter / fadeDuration);  // Lerp alpha from 0 to 1
            faderImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;  // Wait for a frame before continuing
        }
        faderImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1); // Ensure it ends fully visible
    }

    private IEnumerator FadeOut ()
    {
        float counter = 0f;
        Color originalColor = faderImage.color;
        while (counter < fadeDuration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, counter / fadeDuration);
            faderImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        faderImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }
}
