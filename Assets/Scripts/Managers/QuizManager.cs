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
    public Stepper stepper;
    public QuizSummary quizSummary;

    [SerializeField] private GameType gameType;
    private Subject subject;
    [SerializeField] private Question question;

    public AnswersManager answersManager;

    public List<ToriObject> currentObjects { get; private set; }
    private List<ToriObject> usedObjects;
    public int currentObjectIndex { get; private set; }


    [Header("Games")]
    [Header("Chest")]
    public Animator chestLidAnimator;
    public Animator parallelObjectAnimator;

    [Header("Catch")]
    public ClampController clampController;
    public DraggingTutorial draggingTutorial;

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
        if (isTest)
            currentObjects = quizTester.subject.toriObjects;
        else
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
        if (isTest)
            return quizTester.subject.toriObjects;
        else
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


    public void CompleteQuiz ()
    {
        quizSummary.ShowSummary();
        quiz.CompleteQuiz();
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

    public void ShowLoadingScreen ()
    {
        LoadingScreen.Instance.ShowLoadingScreen();
    }

    public void HideLoadingScreen ()
    {
        LoadingScreen.Instance.HideLoadingScreen();
    }


}
