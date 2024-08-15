using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour
{
    public CanvasGroup canvasGroup;  // Reference to the CanvasGroup component
    public float fadeDuration = 1.0f; // Duration of the fade effect

    private Coroutine fadeCoroutine;

    private void Awake ()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    public void ShowObject ()
    {
        canvasGroup.alpha = 1;
        EnableInteractions(true);
    }

    public void HideObject ()
    {
        canvasGroup.alpha = 0;
        EnableInteractions(false);
    }

    public void FadeIn ()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(Fade(0.0f, 1.0f));
        EnableInteractions(true);
    }

    public void FadeOut ()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        EnableInteractions(false);
        fadeCoroutine = StartCoroutine(Fade(1.0f, 0.0f));
    }

    private IEnumerator Fade ( float startAlpha, float endAlpha )
    {
        float elapsedTime = 0.0f;

        // Set the initial alpha
        canvasGroup.alpha = startAlpha;

        // Gradually change the alpha value over the specified duration
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration));
            yield return null;
        }

        // Ensure the final alpha value is set
        canvasGroup.alpha = endAlpha;
    }

    private void EnableInteractions ( bool interactable )
    {
        canvasGroup.interactable = interactable;
        canvasGroup.blocksRaycasts = interactable;
    }
}
