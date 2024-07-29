using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static QuizManager;

public class CatchQuiz : IQuiz
{
    private Question currentQuestion;
    private QuizManager quizManager;
    private ToriObject currentToriObject;

    private Subject subject;
    private int correctAnswersCounter;

    private int levelNumber = 2;


    public void InitiateQuiz ()
    {
        InitiateQuizCoroutine();
    }

    private async void InitiateQuizCoroutine ()
    {
        LoadObjects();
        GetSubjectFromManager();

        await quizManager.answersManager.InstantiateAnswersAsync();

        quizManager.SetAnswersQuizManager();
        ResetAnswers();
        LoadCurrentQuestion();
        DeployAnswers();



        FadeInAnswers();
    }

    private void LoadObjects ()
    {
        quizManager.LoadObjects(levelNumber);
    }

    public void SetQuizManager ( QuizManager _quizManager )
    {
        this.quizManager = _quizManager;
    }

    private void GetSubjectFromManager ()
    {
        subject = SubjectsManager.Instance.selectedSubject;
    }

    public void SetQuestion ( Question question )
    {
        currentQuestion = question;
    }


    public void LoadCurrentQuestion ()
    {
        currentToriObject = quizManager.GetCurrentObject();

        DeployQuestion(currentToriObject);
    }

    public void DeployQuestion ( ToriObject toriObject )
    {
        switch (subject.name)
        {
            case "Colors":
                currentQuestion.ColorImage(toriObject.color);
                break;
            case "Shapes":
                currentQuestion.SetImage(toriObject.sprite);
                break;


        }
    }


    public void AnswerClicked ( bool isCorrect )
    {

    }

    public void NextQuestion ()
    {
        LoadCurrentQuestion();

    }

    public void DeployAnswers ()
    {
        List<Answer> answers = quizManager.answersManager.GetAnswers();

        if (answers == null || answers.Count < 10)
        {
            Debug.LogError("Insufficient number of answers available.");
            return;
        }

        ToriObject correctObject = quizManager.GetCurrentObject();
        List<ToriObject> allObjects = new List<ToriObject>(quizManager.GetAllSubjectObjects());
        Debug.Log(allObjects.Count);
        allObjects.Remove(correctObject);

        List<ToriObject> wrongObjects = GetRandomObjects(allObjects, 7);

        List<ToriObject> answerObjects = new List<ToriObject> { correctObject, correctObject, correctObject };
        answerObjects.AddRange(wrongObjects);

        List<Answer> shuffledAnswers = new List<Answer>(answers);
        ShuffleList(shuffledAnswers);

        for (int i = 0; i < shuffledAnswers.Count; i++)
        {
            bool isCorrect = i < 3;
            Answer answer = shuffledAnswers[i];
            ToriObject toriObject = answerObjects[i];

            DeployAnswer(answer, toriObject);
            if (isCorrect)
            {
                answer.SetAsCorrect();
                answer.SetTarget(currentQuestion.target);
            }
        }
    }

    private List<ToriObject> GetRandomObjects ( List<ToriObject> objects, int count )
    {
        List<ToriObject> randomObjects = new List<ToriObject>();

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, objects.Count);
            randomObjects.Add(objects[randomIndex]);
        }

        return randomObjects;
    }

    private void ShuffleList<T> ( List<T> list )
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }


    private void DeployAnswer ( Answer answer, ToriObject toriObject )
    {
        switch (subject.name)
        {
            case "Colors":
                answer.SetColor(toriObject.color);
                break;
            case "Shapes":
                answer.SetImage(toriObject.sprite);
                break;

        }

        answer.SetAudioClip(toriObject.clip);
    }


    public void CorrectAnswer ()
    {
        Answer currentAnswer = quizManager.answersManager.currentAnswer;
        quizManager.answersManager.FadeOutAnswer(currentAnswer);

        if (correctAnswersCounter == 2)
        {
            quizManager.SetQuestionState(QuestionState.Correct);
            quizManager.feedbackManager.SendFeedback(0);
            correctAnswersCounter = 0;
        }
        else
            correctAnswersCounter++;

    }



    public void WrongAnswer ()
    {
        quizManager.feedbackManager.SendFeedback(1);
    }

    private void ResetAnswers ()
    {
        quizManager.answersManager.ResetAnswers();
    }

    public void CorrectFeedbackClicked ()
    {
        ResetAnswers();

        quizManager.ResetUnusedAnswersList();
        quizManager.MoveToNextObject();
        InitiateQuiz();
    }

    public void WrongFeedbackClicked ()
    {
        DeployQuestion(currentToriObject);
    }

    public void CompleteQuiz ()
    {
        FadeOutAnswers();
        GameManager.Instance.SaveProgress();
    }

    public void FadeInAnswers ()
    {
        quizManager.answersManager.FadeInAnswers();
    }

    public void FadeOutAnswers ()
    {
        quizManager.answersManager.FadeOutAnswers();
    }

}
