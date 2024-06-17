using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswersManager : MonoBehaviour
{
    public FeedbackManager feedbackManager;
    public LevelManager levelManager;
    public DialogManager dialogManager;

    [SerializeField] private List<Answer> answers;
    [SerializeField] private Transform answersParent;
    [SerializeField] private bool isToriAskQuestions;

    public Answer currentCorrectAnswer { get; private set; }
    private List<ToriObject> toriObjects;
    private bool isAnswerCorrect;

    public delegate void AnswersManagerEventHandler ();
    public event AnswersManagerEventHandler OnAnswersManagerReady;
    public event AnswersManagerEventHandler OnCorrectDraggableTarget;


    private void OnEnable ()
    {
        dialogManager.OnFeedbackClicked += FeedbackClicked;
    }

    private void OnDisable ()
    {
        dialogManager.OnFeedbackClicked -= FeedbackClicked;
    }


    private void Start ()
    {
        LoadLevelObjects();
        SetAnswers();
    }

    private void LoadLevelObjects ()
    {
        toriObjects?.Clear();
        toriObjects = GameManager.Instance.currentLevelObjects;
    }

    public void SetAnswers ()
    {
        List<ToriObject> availableToriObjects = new List<ToriObject>(toriObjects);

        for (int i = 0; i < answers.Count; i++)
        {
            answers[i].ResetAnswer();

            if (i < availableToriObjects.Count)
            {
                ToriObject toriObject = availableToriObjects[i];

                answers[i].SetAnswer(toriObject);
                answers[i].SetAnswersManager(this);
            }
        }

        currentCorrectAnswer = answers[levelManager.stepper.currentStep % answers.Count];

        currentCorrectAnswer.SetAsCorrect();

        SendToriQuestion(currentCorrectAnswer);

        OnAnswersManagerReady?.Invoke();
    }

    private void SendToriQuestion ( Answer correctAnswer )
    {
        dialogManager.toriTheCat.SetEmotion("Question");

        if (!isToriAskQuestions) return;

        dialogManager.SetAnswerAndPlay(correctAnswer);
    }

    public void CorrectDraggableTarget ()
    {
        OnCorrectDraggableTarget.Invoke();
    }

    public void CorrectAnswer ()
    {
        isAnswerCorrect = true;
        dialogManager.FadeOut();
        levelManager.NextStep();

        if (levelManager.IsLastStep())
            feedbackManager.SendFeedback(2);
        else
            feedbackManager.SendFeedback(0);
    }

    public void WrongAnswer ()
    {
        isAnswerCorrect = false;
        dialogManager.FadeOut();
        feedbackManager.SendFeedback(1);
    }

    private void FeedbackClicked ()
    {
        if (isAnswerCorrect)
        {
            if (levelManager.IsLastStep())
            {
                levelManager.CompleteLevel();
                return;
            }

            SetAnswers();
        }
        else
        {
            SendToriQuestion(currentCorrectAnswer);
        }
        Debug.Log("isanswercorrect = " + isAnswerCorrect);
    }

    public List<Answer> GetAnswerList ()
    {
        return answers;
    }


}
