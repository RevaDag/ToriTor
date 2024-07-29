using static GameManager;
using static SubjectsManager;
using System.Collections.Generic;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    private IQuizFactory quizFactory;
    private IQuiz quiz;

    public enum QuestionState
    {
        Pending,
        Correct,
        Wrong
    }

    public QuestionState currentQuestionState;

    public FeedbackManager feedbackManager;
    public DialogManager dialogManager;
    public Stepper stepper;
    public QuizSummary quizSummary;

    [SerializeField] private GameType gameType;
    private Subject subject;
    [SerializeField] private Question question;

    public AnswersManager answersManager;

    public List<ToriObject> currentObjects { get; private set; }
    private List<ToriObject> usedObjects;
    private int currentObjectIndex;

    public Animator chestLidAnimator;
    public Animator parallelObjectAnimator;

    [Header("Test")]
    public bool isTest;
    public QuizTester quizTester;

    void Start ()
    {
        quizFactory = new QuizFactory();
        quiz = quizFactory.CreateQuiz(gameType);

        usedObjects = new List<ToriObject>();

        InitiateQuiz();
        SetAnswersQuizManager();
    }

    private void InitiateQuiz ()
    {
        quiz.SetQuizManager(this);
        quiz.SetQuestion(question);

        quiz.InitiateQuiz();
    }

    public void LoadObjects ( int listNumber )
    {
        currentObjects = SubjectsManager.Instance.GetObjectsByListNumber(listNumber);
    }


    public void SetAnswersQuizManager ()
    {
        foreach (var answer in answersManager.GetActiveAnswers())
        {
            answer.SetQuizManager(this);
        }
    }

    public List<ToriObject> GetAllSubjectObjects ()
    {
        return SubjectsManager.Instance.selectedSubject.toriObjects;
    }

    public ToriObject GetCurrentObject ()
    {
        ToriObject obj = currentObjects[currentObjectIndex];
        AddObjectToUsuedObjectList(obj);
        return obj;
    }

    public void MoveToNextObject ()
    {
        ResetQuestionState();
        ResetToriEmoji();
        currentObjectIndex++;
    }

    private void AddObjectToUsuedObjectList ( ToriObject usedObject )
    {
        if (usedObjects.Contains(usedObject)) return;

        usedObjects.Add(usedObject);
    }


    public void ResetUnusedAnswersList ()
    {
        answersManager.ResetUnusedAnswersList();
    }


    public List<ToriObject> GetRandomObjects ( int numberOfObjects, ToriObject exceptThisObject )
    {
        List<ToriObject> tempObjects = new List<ToriObject>(currentObjects);
        tempObjects.Remove(exceptThisObject);

        if (tempObjects.Count < numberOfObjects)
        {
            Debug.LogError("Not enough objects to select from.");
            return tempObjects;
        }

        List<ToriObject> objList = new List<ToriObject>();

        for (int i = 0; i < numberOfObjects; i++)
        {
            int randomIndex = Random.Range(0, tempObjects.Count);
            objList.Add(tempObjects[randomIndex]);
            tempObjects.RemoveAt(randomIndex);
        }

        return objList;
    }

    public void CorrectAnswer ()
    {
        quiz.CorrectAnswer();
    }

    public void WrongAnswer ()
    {
        SetQuestionState(QuestionState.Wrong);
        quiz.WrongAnswer();
    }

    public void ResetQuestionState ()
    {
        SetQuestionState(QuestionState.Pending);
    }

    public void SetQuestionState ( QuestionState questionState )
    {
        currentQuestionState = questionState;
    }

    public void AnswerClicked ( bool isCorrect )
    {
        quiz.AnswerClicked(isCorrect);
    }

    public void OnFeedbackClicked ()
    {
        switch (currentQuestionState)
        {
            case QuestionState.Correct:
                if (usedObjects.Count < currentObjects.Count)
                    quiz.CorrectFeedbackClicked();
                else
                    CompleteQuiz();
                break;

            case QuestionState.Wrong:
                quiz.WrongFeedbackClicked();
                break;

            case QuestionState.Pending:
                dialogManager.StartTalking();
                break;

        }
    }

    private void CompleteQuiz ()
    {
        quizSummary.ShowSummary();
        quiz.CompleteQuiz();
    }

    private void ResetToriEmoji ()
    {
        dialogManager.toriTheCat.SetEmotion("Default");
    }

    public void ResetQuiz ()
    {
        usedObjects.Clear();
        answersManager.ResetUnusedAnswersList();
        currentObjectIndex = 0;
        ResetQuestionState();
        quiz.InitiateQuiz();
        quizSummary.ResetStickers();
        quizSummary.HideSummary();
    }


}
