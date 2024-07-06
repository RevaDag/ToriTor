using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameManager;

public class QuestionAnswerManager : MonoBehaviour
{

    [SerializeField] private GameType gameType;

    public FeedbackManager feedbackManager;
    public LevelManager levelManager;
    public DialogManager dialogManager;
    public Answer currentQuestion;

    [SerializeField] private List<Answer> answers;
    [SerializeField] private Transform answersParent;
    [SerializeField] private bool isToriAskQuestions;

    public Answer currentCorrectAnswer { get; private set; }

    private List<ToriObject> toriObjects;
    private List<ToriObject> selectedObjects = new List<ToriObject>();
    private List<ToriObject> unselectedObjects;
    private Dictionary<Answer, Answer> answerToQuestionMap;
    private int currentAnswerIndex = 0;



    private bool isAnswerCorrect;

    public delegate void AnswersManagerEventHandler ();
    public event AnswersManagerEventHandler OnAnswersManagerReady;
    public event AnswersManagerEventHandler OnCorrectDraggableTarget;


    private void Start ()
    {
        LoadLevelObjects();

        if (gameType != GameType.Matching)
            SetAnswers();
        else
            DeployQuestionsAndAnswers();
    }


    private void LoadLevelObjects ()
    {
        toriObjects = new List<ToriObject>(GameManager.Instance.selectedObjects);
        unselectedObjects = new List<ToriObject>(toriObjects);
    }

    public void SetAnswers ()
    {
        // Ensure we have enough ToriObjects to cover all answer slots
        if (unselectedObjects.Count < answers.Count)
        {
            // Reset the unselected list when all objects have been used
            unselectedObjects = new List<ToriObject>(toriObjects);
        }

        List<ToriObject> availableForCorrect = unselectedObjects.Except(selectedObjects).ToList();

        if (availableForCorrect.Count == 0)
        {
            // Reset usedCorrectAnswers if all objects have been used as correct answers
            selectedObjects.Clear();
            availableForCorrect = new List<ToriObject>(unselectedObjects);
        }

        // Select the correct answer from available ToriObjects not used as correct before
        ToriObject correctAnswer = availableForCorrect[0];
        selectedObjects.Add(correctAnswer);
        unselectedObjects.Remove(correctAnswer);

        // Select other answers from the remaining unselected ToriObjects
        List<ToriObject> wrongAnswers = new List<ToriObject>();
        for (int i = 0; i < answers.Count - 1; i++)
        {
            ToriObject wrongAnswer = unselectedObjects[0];
            unselectedObjects.RemoveAt(0);
            wrongAnswers.Add(wrongAnswer);
        }

        // Shuffle answers to mix correct and wrong answers
        List<ToriObject> allAnswers = new List<ToriObject> { correctAnswer };
        allAnswers.AddRange(wrongAnswers);
        allAnswers = allAnswers.OrderBy(a => Random.value).ToList();

        // Assign answers to answer slots
        for (int i = 0; i < answers.Count; i++)
        {
            answers[i].ResetAnswer();
            // answers[i].SetQuestionAnswer(allAnswers[i], gameType);
            // answers[i].SetAnswersManager(this);
        }

        // Find and set the correct answer
        currentCorrectAnswer = answers.First(a => a.GetToriObject() == correctAnswer);
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

    private void DeployQuestionsAndAnswers ()
    {
        answerToQuestionMap = new Dictionary<Answer, Answer>();

        Answer lastSelectedAnswer = null;

        if (currentAnswerIndex >= answers.Count)
        {
            currentAnswerIndex = 0;
        }

        Answer currentAnswer = answers[currentAnswerIndex];

        if (currentAnswer == lastSelectedAnswer)
        {
            // Select the next answer in the list
            currentAnswerIndex = (currentAnswerIndex + 1) % answers.Count;
            currentAnswer = answers[currentAnswerIndex];
        }

        lastSelectedAnswer = currentAnswer;

        SetQuestionAndAssignToAnswer(currentAnswer, currentQuestion);

        currentAnswerIndex = (currentAnswerIndex + 1) % answers.Count;
    }


    private void SetQuestionAndAssignToAnswer ( Answer answer, Answer question )
    {
        //question.SetQuestionAnswer(answer.toriObject, gameType);
        answer.draggable?.SetTarget(question.target);
        answerToQuestionMap[answer] = question;
    }

    public void CorrectDraggableTarget ()
    {
        OnCorrectDraggableTarget?.Invoke();
        currentCorrectAnswer.audioSource.Play();
    }

    public void CorrectAnswer ()
    {
        isAnswerCorrect = true;
        dialogManager.FadeOut();
        levelManager.NextStep();

        SendFeedback();
    }

    private void SendFeedback ()
    {
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
