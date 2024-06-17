using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public ToriTheCat toriTheCat;

    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private Image dialogArrowImage;
    [SerializeField] private float delay = 0.1f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Fader dialogFader;

    private List<Line> lines;
    private int currentLineIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    #region Events
    public delegate void DialogClickedEventHandler ();
    public event DialogClickedEventHandler OnFeedbackClicked;
    public event DialogClickedEventHandler OnQuestionClicked;
    #endregion

    #region Fader

    public void FadeIn ()
    {
        dialogFader.FadeIn();
    }

    public void FadeOut ()
    {
        dialogFader.FadeOut();
    }

    #endregion

    private void Start ()
    {
        dialogArrowImage.enabled = false;
    }

    public void SetAnswerAndPlay ( Answer correctAnswer )
    {

        Line questionLine = new Line();
        questionLine.text = correctAnswer.toriObject.objectName;
        questionLine.audioClip = correctAnswer.toriObject.clip;
        questionLine.type = Line.Type.Question;

        List<Line> newLines = new List<Line>();
        newLines.Add(questionLine);

        SetLinesAndPlay(newLines);
    }

    public void SetLinesAndPlay ( List<Line> lineList )
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
        dialogFader.FadeIn();
        SayNextLine();
    }


    public void SayNextLine ()
    {
        if (!isTyping)
        {
            typingCoroutine = StartCoroutine(TypeText(lines[currentLineIndex]));
        }
    }

    private void SayPreviousLine ()
    {
        if (!isTyping)
        {
            typingCoroutine = StartCoroutine(TypeText(lines[currentLineIndex - 1]));
        }
    }

    private IEnumerator TypeText ( Line line )
    {
        dialogArrowImage.enabled = false;
        isTyping = true;
        audioSource.clip = line.audioClip;
        audioSource.Play();

        dialogText.text = "";

        foreach (char letter in line.text.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(delay);
        }

        isTyping = false;
        dialogArrowImage.enabled = true;

        currentLineIndex++;
    }

    public void OnDialogBoxClick ()
    {
        if (isTyping)
        {
            // If currently typing, reveal the full current line
            isTyping = false;
            StopCoroutine(typingCoroutine);
            dialogText.text = lines[currentLineIndex].text;
            currentLineIndex++;
            return;
        }
        else if (currentLineIndex < lines.Count)
        {
            SayNextLine();
        }
        else
        {
            switch (lines[currentLineIndex - 1].type)
            {
                case Line.Type.Dialog:
                    dialogFader.FadeOut();
                    break;

                case Line.Type.Feedback:
                    OnFeedbackClicked?.Invoke();
                    dialogFader.FadeOut();
                    break;

                case Line.Type.Question:
                    SayPreviousLine();
                    OnQuestionClicked?.Invoke();
                    break;
            }
        }

        OnFeedbackClicked?.Invoke();
    }


}
