using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BookCatalog : MonoBehaviour
{
    [SerializeField] private CanvasGroup sceneFadeCanvasGroup;
    [SerializeField] private float fadeDuration = .5f;

    public void OpenBook ( string bookName )
    {
        StartCoroutine(OpenBookCoroutine(bookName));
    }

    private IEnumerator OpenBookCoroutine ( string bookName )
    {
        if (bookName == null) yield return null;
        StartCoroutine(FadeIn(sceneFadeCanvasGroup));
        SceneManager.LoadScene(bookName);
        yield return null;
    }

    private IEnumerator FadeIn ( CanvasGroup canvasGroup )
    {
        float counter = 0f;
        while (counter < fadeDuration)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, counter / fadeDuration);
            yield return null;
        }
    }

    private IEnumerator FadeOut ( CanvasGroup canvasGroup )
    {
        float counter = 0f;
        while (counter < fadeDuration)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, counter / fadeDuration);
            yield return null;
        }
    }
}
