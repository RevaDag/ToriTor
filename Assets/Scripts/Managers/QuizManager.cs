using static GameManager;
using static SubjectsManager;
using System.Collections.Generic;
using UnityEngine;

public class QuizManager : MonoBehaviour, IGame
{
    private IQuizFactory quizFactory;
    private IQuiz quiz;

    public FeedbackManager feedbackManager;
    public GameSummary quizSummary;

    [SerializeField] private GameType gameType;
    private Subject subject;
    public List<Question> questions;

    public AnswersManager answersManager;

    [Header("Audio")]
    [SerializeField] private AudioClip levelThemeMusic;
    public AudioClip correctClip;
    public AudioClip bubbles;
    private AudioSource audioSource;

    public List<ToriObject> currentObjects { get; private set; }
    private List<ToriObject> usedObjects;
    public int currentObjectIndex { get; private set; }

    private bool firstLoad = true;


    [Header("Games")]
    [Header("Chest")]
    public Animator chestLidAnimator;
    public Animator parallelObjectAnimator;

    [Header("Catch")]
    public ClampController clampController;
    public DraggingTutorial draggingTutorial;

    [Header("Find")]
    public List<Fader> objectFaders;

    [Header("Test")]
    public QuizTester quizTester;

    private void Awake ()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start ()
    {
        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayAudioClip(levelThemeMusic);


        quizFactory = new QuizFactory();
        quiz = quizFactory.CreateQuiz(gameType);

        usedObjects = new List<ToriObject>();


        InitiateQuiz();
    }

    private void InitiateQuiz ()
    {
        quiz.SetQuizManager(this);
        quizSummary.SetGameInterface(this);
        quiz.InitiateQuiz();
    }

    public List<ToriObject> LoadLevelObjects ()
    {
        if (quizTester.isTest)
            currentObjects = quizTester.selectedObjects;
        else
            currentObjects = SubjectsManager.Instance.GetObjectsByListNumber(quizSummary.levelNumber);

        return currentObjects;

    }


    public Subject GetSubject ()
    {
        if (quizTester.isTest)
            subject = quizTester.subject;
        else
            subject = SubjectsManager.Instance.selectedSubject;

        return subject;
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

    public Question GetQuestionWithToriObject ( ToriObject toriObject )
    {
        foreach (Question question in questions)
        {
            if (question.toriObject == toriObject) // Assuming Question has a property toriObject
            {
                return question;
            }
        }
        return null; // Return null if no matching question is found
    }


    public void CorrectAnswer ( Answer answer )
    {
        quiz.CorrectAnswer(answer);

        PlayClip(correctClip, .3f);
    }


    public void PlayClip ( AudioClip _clip, float _volume )
    {
        audioSource.clip = _clip;
        audioSource.volume = _volume;
        audioSource.Play();
    }

    public void WrongAnswer ()
    {
        quiz.WrongAnswer();
    }


    public void CompleteQuiz ()
    {
        quizSummary.ShowSummary();
        answersManager.FadeOutAnswers();
        GameManager.Instance.CompleteLevelAndProgressToNextLevel(quizSummary.levelNumber);
    }


    public void ResetGame ()
    {
        usedObjects.Clear();
        answersManager.ResetUnusedAnswersList();
        currentObjectIndex = 0;
        quiz.ResetQuiz();
        quizSummary.HideSummary();
    }

    public void OnParallelObjectClick ()
    {
        quiz.NextQuestion();
    }

    #region Hide & Show Loading Screen

    public void ShowLoadingScreen ()
    {
        LoadingScreen.Instance.ShowLoadingScreen();
    }

    public void HideLoadingScreen ()
    {
        if (LoadingScreen.Instance != null && firstLoad)
        {
            firstLoad = false;
            LoadingScreen.Instance.HideLoadingScreen();
        }
    }

    #endregion


}
