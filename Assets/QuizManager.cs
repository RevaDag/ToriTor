using static GameManager;
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
    [SerializeField] private Question question;
    [SerializeField] private List<Answer> answers;

    private List<ToriObject> selectedObjects;
    private List<Answer> unusedAnswers;
    private List<ToriObject> usedObjects;
    private int currentObjectIndex;

    void Start ()
    {
        quizFactory = new QuizFactory();
        selectedObjects = GameManager.Instance.selectedObjects;
        unusedAnswers = new List<Answer>(answers);
        usedObjects = new List<ToriObject>();
        quiz = quizFactory.CreateQuiz(gameType);

        quiz.SetQuizManager(this);
        quiz.SetQuestion(question);
        quiz.SetAnswers(answers);
        SetAnswersQuizManager();
        quiz.InitiateQuiz();
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

    public void OnFeedbackClicked ()
    {
        switch (currentQuestionState)
        {
            case QuestionState.Correct:
                if (usedObjects.Count < selectedObjects.Count)
                    quiz.CorrectFeedbackClicked();
                else
                    quizSummary.ShowSummary();
                break;

            case QuestionState.Wrong:
                quiz.WrongFeedbackClicked();
                break;

            case QuestionState.Pending:
                dialogManager.StartTalking();
                break;

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
        quizSummary.HideSummary();
    }


}
