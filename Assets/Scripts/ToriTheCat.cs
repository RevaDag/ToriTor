using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToriTheCat : MonoBehaviour
{


    [SerializeField] private TMP_Text textBubble;
    [SerializeField] private List<Line> lines;
    [SerializeField] private float delay = 0.1f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Fader toriParentFader;
    [SerializeField] private Fader textBubbleFader;

    private int currentLineIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    public delegate void BubbleClickedEventHandler ();
    public event BubbleClickedEventHandler OnBubbleClicked;


    private void Start ()
    {
        textBubble.text = "";

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

    public void SetLineListAndPlay ( List<Line> lineList )
    {
        if (lineList == null || lineList.Count == 0)
        {
            Debug.LogWarning("Line list is empty or null.");
            return;
        }

        lines = lineList;
        currentLineIndex = 0;
        StartTalking();
    }

    private void StartTalking ()
    {
        FadeIn(toriParentFader);
        FadeIn(textBubbleFader);
        StartNextLine();
    }


    public void StartNextLine ()
    {
        if (!isTyping && currentLineIndex < lines.Count)
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

    public void OnBubbleClick ()
    {
        if (isTyping)
        {
            // If currently typing, reveal the full current line
            StopCoroutine(typingCoroutine);
            textBubble.text = lines[currentLineIndex].text;
            isTyping = false;
            currentLineIndex++;
        }
        else if (currentLineIndex < lines.Count)
        {
            // If not typing, start typing the next line
            StartNextLine();
        }
        else
        {
            FadeOut(textBubbleFader);
        }

        OnBubbleClicked?.Invoke();
    }

    public void ShowFeedback ( Line feedback )
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            isTyping = false;
        }

        textBubble.text = feedback.text;
        audioSource.Stop(); // Stop any currently playing audio clip

        if (feedback.audioClip != null)
        {
            audioSource.clip = feedback.audioClip;
            audioSource.Play();
        }

        FadeIn(toriParentFader);
        FadeIn(textBubbleFader);
    }


}
