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
    [SerializeField] private List<Answer> answers;

    private List<ToriObject> selectedObjects;
    private List<Answer> unusedAnswers;
    private List<ToriObject> usedObjects;
    private int currentObjectIndex;

    public Animator chestLidAnimator;

    [Header("Test")]
    public bool isTest;
    public QuizTester quizTester;

    void Start ()
    {
        quizFactory = new QuizFactory();
        quiz = quizFactory.CreateQuiz(gameType);

        LoadObjectsAndSubject();

        unusedAnswers = new List<Answer>(answers);
        usedObjects = new List<ToriObject>();

        InitiateQuiz();
        SetAnswersQuizManager();
    }

    private void InitiateQuiz ()
    {
        quiz.SetQuizManager(this);
        quiz.SetSubject(subject);
        quiz.SetQuestion(question);
        quiz.SetAnswers(answers);
        quiz.InitiateQuiz();
    }

    private void LoadObjectsAndSubject ()
    {
        if (isTest)
        {
            selectedObjects = quizTester.testObjects;
            subject = quizTester.subject;
        }
        else
        {
            selectedObjects = GameManager.Instance.selectedObjects;
            subject = GameManager.Instance.currentSubject;
        }
    }

    private void SetAnswersQuizManager ()
    {
        foreach (var answer in answers)
        {
            answer.SetQuizManager(this);
        }
    }

    public ToriObject GetCurrentObject ()
    {
        ToriObject obj = selectedObjects[currentObjectIndex];
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

    public Answer GetUnusedAnswer ()
    {
        if (unusedAnswers.Count == 0)
        {
            ResetUnusedAnswersList();
        }

        int randomIndex = Random.Range(0, unusedAnswers.Count);
        Answer answer = unusedAnswers[randomIndex];
        unusedAnswers.RemoveAt(randomIndex);
        return answer;
    }


    public void ResetUnusedAnswersList ()
    {
        unusedAnswers = new List<Answer>(answers);
        unusedAnswers.AddRange(answers);
    }

    public List<ToriObject> GetRandomObjects ( int numberOfObjects, ToriObject exceptThisObject )
    {
        // Create a copy of the selectedObjects list
        List<ToriObject> tempObjects = new List<ToriObject>(selectedObjects);

        // Remove the specified object from the temp list
        tempObjects.Remove(exceptThisObject);

        // Create a new list to store the random objects
        List<ToriObject> objList = new List<ToriObject>();

        for (int i = 0; i < numberOfObjects; i++)
        {
            int randomIndex = Random.Range(0, tempObjects.Count);
            objList.Add(tempObjects[randomIndex]);
            tempObjects.RemoveAt(randomIndex);  // Remove the selected object to avoid duplicates
        }

        return objList;
    }

    public void CorrectAnswer ()
    {
        currentQuestionState = QuestionState.Correct;
        quiz.CorrectAnswer();
    }

    public void WrongAnswer ()
    {
        currentQuestionState = QuestionState.Wrong;
        quiz.WrongAnswer();
    }

    public void ResetQuestionState ()
    {
        currentQuestionState = QuestionState.Pending;
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
                if (usedObjects.Count < selectedObjects.Count)
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

        if (gameType == GameType.Chest)
        {
            if (isTest)
                quizSummary.SetObjects(quizTester.testObjects);
            else
                quizSummary.SetObjects(selectedObjects);

            quizSummary.InstantiateStickers();
        }
    }

    private void ResetToriEmoji ()
    {
        dialogManager.toriTheCat.SetEmotion("Default");
    }

    public void ResetQuiz ()
    {
        usedObjects.Clear();
        ResetUnusedAnswersList();
        currentObjectIndex = 0;
        ResetQuestionState();
        quiz.InitiateQuiz();
        quizSummary.ResetStickers();
        quizSummary.HideSummary();
    }


}
