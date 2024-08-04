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
    public QuizSummary quizSummary;

    [SerializeField] private GameType gameType;
    private Subject subject;
    public List<Question> questions;

    public AnswersManager answersManager;

    [Header("Audio")]
    [SerializeField] private AudioClip levelThemeMusic;
    public AudioClip correctClip;
    public AudioClip successClip;
    public AudioClip bubbles;
    private AudioSource audioSource;

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
    public QuizTester quizTester;

    private void Awake ()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start ()
    {
        MusicManager.Instance.PlayAudioClip(levelThemeMusic);


        quizFactory = new QuizFactory();
        quiz = quizFactory.CreateQuiz(gameType);

        usedObjects = new List<ToriObject>();

        InitiateQuiz();

        _ = LoadingScreen.Instance.HideLoadingScreen();

    }

    private void InitiateQuiz ()
    {
        quiz.SetQuizManager(this);

        quiz.InitiateQuiz();
    }



    public List<ToriObject> LoadObjects ( int listNumber )
    {
        if (quizTester.isTest)
            currentObjects = quizTester.selectedObjects;
        else
            currentObjects = SubjectsManager.Instance.GetObjectsByListNumber(listNumber);

        return currentObjects;
    }


    public List<ToriObject> GetAllSubjectObjects ()
    {
        if (quizTester.isTest)
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

    public void CorrectAnswer ( Answer answer )
    {
        quiz.CorrectAnswer(answer);

        PlayClip(correctClip);
    }


    public void PlayClip ( AudioClip _clip )
    {
        audioSource.clip = _clip;
        audioSource.Play();
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

    public void CompleteQuiz ()
    {
        PlayClip(successClip);

        quizSummary.ShowSummary();
        answersManager.FadeOutAnswers();

        GameManager.Instance.CompleteLevelAndProgressToNextLevel(quizSummary.levelNumber);
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

    #region Hide & Show Loading Screen

    public void ShowLoadingScreen ()
    {
        LoadingScreen.Instance.ShowLoadingScreen();
    }

    public void HideLoadingScreen ()
    {
        _ = LoadingScreen.Instance.HideLoadingScreen();
    }

    #endregion


}
