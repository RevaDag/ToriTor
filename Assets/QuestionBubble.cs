using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionBubble : MonoBehaviour
{
    [SerializeField] private TMP_Text questionText;

    private AudioSource audioSource;
    private Fader fader;

    private void Awake ()
    {
        audioSource = GetComponent<AudioSource>();
        fader = GetComponent<Fader>();
    }

    private void Start ()
    {
        audioSource.Play();
    }

    #region Fader

    public void FadeIn ( Fader fader )
    {
        fader?.FadeIn();
    }

    public void FadeOut ( Fader fader )
    {
        fader?.FadeOut();
    }

    #endregion

    public void SetQuestionBubble ( ToriObject toriObject )
    {
        questionText.text = toriObject.objectName;
        audioSource.clip = toriObject.clip;
        FadeIn(fader);
    }

    public void OnClickQuestionBubble ()
    {
        audioSource.Play();
    }

}

