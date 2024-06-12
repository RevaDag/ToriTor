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

    public void FadeIn ()
    {
        fader?.FadeIn();
    }

    public void FadeOut ()
    {
        fader?.FadeOut();
    }

    #endregion

    public void SetQuestionBubble ( ToriObject toriObject )
    {
        questionText.text = toriObject.objectName;
        audioSource.clip = toriObject.clip;
        FadeIn();
    }

    public void OnClickQuestionBubble ()
    {
        audioSource.Play();
    }

}

