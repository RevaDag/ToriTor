using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToriTheCat : MonoBehaviour
{
    [System.Serializable]
    public class Line
    {
        public string text;
        public AudioClip audioClip;
    }

    [SerializeField] private TMP_Text textBubble;
    [SerializeField] private Line[] lines;
    [SerializeField] private float delay = 0.1f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Fader toriParentFader;
    [SerializeField] private Fader textBubbleFader;

    private int currentLineIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;


    private void Start ()
    {
        textBubble.text = "";

        //StartTalking();
    }

    private void StartTalking ()
    {
        FadeIn(toriParentFader);
        FadeIn(textBubbleFader);
        StartNextLine();
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

    public void StartNextLine ()
    {
        if (!isTyping && currentLineIndex < lines.Length)
        {
            typingCoroutine = StartCoroutine(TypeText(lines[currentLineIndex]));
        }
    }

    private IEnumerator TypeText ( Line line )
    {
        isTyping = true;
        audioSource.clip = line.audioClip;
        audioSource.Play();

        textBubble.text = "";

        foreach (char letter in line.text.ToCharArray())
        {
            textBubble.text += letter;
            yield return new WaitForSeconds(delay);
        }

        isTyping = false;
        currentLineIndex++;
    }

    public void OnSpeechBubbleClick ()
    {
        if (isTyping)
        {
            // If currently typing, reveal the full current line
            StopCoroutine(typingCoroutine);
            textBubble.text = lines[currentLineIndex].text;
            isTyping = false;
            currentLineIndex++;
        }
        else if (currentLineIndex < lines.Length)
        {
            // If not typing, start typing the next line
            StartNextLine();
        }
        else
        {
            FadeOut(toriParentFader);
        }
    }

}
