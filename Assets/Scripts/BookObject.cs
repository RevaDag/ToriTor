using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookObject : MonoBehaviour
{
    [SerializeField] private GameObject objectTextEng;
    [SerializeField] private GameObject objectTextHeb;
    [SerializeField] private AudioClip hebrewWord;
    [SerializeField] private AudioClip englishWord;
    [SerializeField] private AudioSource wordAudioSource;

    [SerializeField] private float fadeDuration = .5f;
    [SerializeField] private CanvasGroup wordCanvasGroup;
    [SerializeField] private CanvasGroup imageCanvasGroup;


    public void FadeInImage ()
    {
        StartCoroutine(FadeIn(imageCanvasGroup));
    }

    public void ChangeLanguage ( int languageNumber )
    {
        StartCoroutine(ChangeLanguageCoroutine(languageNumber));
    }

    private IEnumerator ChangeLanguageCoroutine ( int languageNumber )
    {
        yield return StartCoroutine(FadeOut(wordCanvasGroup));

        switch (languageNumber)
        {
            case 0: // Hebrew
                objectTextEng.SetActive(false);
                objectTextHeb.SetActive(true);
                wordAudioSource.clip = hebrewWord;
                break;

            case 1: // English
                objectTextEng.SetActive(true);
                objectTextHeb.SetActive(false);
                wordAudioSource.clip = englishWord;
                break;
        }

        yield return StartCoroutine(FadeIn(wordCanvasGroup));
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

    public void SayTheWord ()
    {
        StartCoroutine(SayTheWordCoroutine());
    }

    private IEnumerator SayTheWordCoroutine ()
    {
        if (wordAudioSource.clip != null)
        {
            yield return new WaitForSeconds(1);
            wordAudioSource.Play();
            yield return null;
        }

    }


}
