using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public ToriTheCat toriTheCat;

    [SerializeField] private QuizManager quizManager;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private Image dialogArrowImage;
    [SerializeField] private float delay = 0.1f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Fader dialogFader;
    [SerializeField] private Button dialogBoxButton;

    private List<Line> lines;
    private int currentLineIndex;
    private bool isTyping;
    private Coroutine typingCoroutine;

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
        var questionLine = new Line
        {
            text = correctAnswer.toriObject.objectName,
            audioClip = correctAnswer.toriObject.clip,
            type = Line.Type.Question
        };

        SetLinesAndPlay(new List<Line> { questionLine });
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

    public void StartTalking ()
    {
        dialogFader.FadeIn();
        SayCurrentLine();
    }

    public void SayCurrentLine ()
    {
        if (!isTyping && currentLineIndex < lines.Count)
        {
            typingCoroutine = StartCoroutine(TypeText(lines[currentLineIndex]));
        }
    }

    private void SayPreviousLine ()
    {
        typingCoroutine = StartCoroutine(TypeText(lines[currentLineIndex - 1]));
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

        if (currentLineIndex < lines.Count)
        {
            currentLineIndex++;
        }

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
        }
        else if (currentLineIndex < lines.Count)
        {
            SayCurrentLine();
            currentLineIndex++;
        }
        else
        {
            HandleCurrentLineType();
        }
    }

    private void HandleCurrentLineType ()
    {
        if (currentLineIndex - 1 >= 0)
        {
            switch (lines[currentLineIndex - 1].type)
            {
                case Line.Type.Dialog:
                    dialogFader.FadeOut();
                    break;

                case Line.Type.Feedback:
                    dialogFader.FadeOut();
                    FeedbackClicked();
                    break;

                case Line.Type.Question:
                    SayPreviousLine();
                    break;
            }
        }
    }

    private void FeedbackClicked ()
    {
        quizManager.OnFeedbackClicked();
    }

    public void ClearDialog ()
    {
        StopAllCoroutines();
        dialogText.ClearMesh();
    }

    public void ActivateDialogButton ( bool isActive )
    {
        dialogBoxButton.enabled = isActive;
    }
}
