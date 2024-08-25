using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static QuizManager;

public class CatchQuiz : IQuiz
{
    private QuizManager quizManager;
    private ToriObject currentToriObject;

    private int correctAnswersCounter;


    public void InitiateQuiz ()
    {
        InitiateQuizCoroutine();
    }

    private async void InitiateQuizCoroutine ()
    {
        quizManager.LoadLevelObjects();

        await InstantiateAnswers();

        quizManager.HideLoadingScreen();

        LoadCurrentQuestion();

        DeployAnswers();

        if (quizManager.clampController != null)
            quizManager.clampController.OpenClamp();

        FadeInAnswers();
    }


    public void ResetQuiz ()
    {
        ResetAnswers();
        InitiateQuiz();
    }


    private async Task InstantiateAnswers ()
    {
        // If this is a tutorial 
        if (quizManager.currentObjectIndex == 0)
            await quizManager.answersManager.InstantiateAnswersAsync();
        else
            await quizManager.answersManager.InstantiateAnswersAsync(8);
    }

    public void SetQuizManager ( QuizManager _quizManager )
    {
        this.quizManager = _quizManager;
    }

    public void LoadCurrentQuestion ()
    {
        currentToriObject = quizManager.GetCurrentObject();

        DeployQuestion(currentToriObject);
    }

    public void DeployQuestion ( ToriObject toriObject )
    {
        Question currentQuestion = quizManager.questions[0];


        if (quizManager.GetSubject().name == "Colors")
            currentQuestion.ColorImage(toriObject.color);

        currentQuestion.SetAudioClip(toriObject.clip);
        currentQuestion.PlayAudioSouce();
    }


    public void DeployAnswers ()
    {
        List<Answer> answers = quizManager.answersManager.GetAnswers();

        if (answers == null || answers.Count < 3)
        {
            Debug.LogError("Insufficient number of answers available.");
            return;
        }

        ToriObject correctObject = quizManager.GetCurrentObject();
        List<ToriObject> allObjects = new List<ToriObject>(quizManager.GetAllSubjectObjects());
        allObjects.Remove(correctObject);

        List<ToriObject> answerObjects = new List<ToriObject> { correctObject, correctObject, correctObject };

        // If this is not a tutorial 
        if (quizManager.currentObjectIndex > 0)
        {
            AddWrongAnswers(allObjects, answerObjects);
        }


        List<Answer> shuffledAnswers = new List<Answer>(answers);
        ShuffleList(shuffledAnswers);
        SetAnswerAsTutorial(shuffledAnswers[0]);

        quizManager.PlayClip(quizManager.bubbles, 1f);

        for (int i = 0; i < shuffledAnswers.Count; i++)
        {
            bool isCorrect = i < 3;
            Answer answer = shuffledAnswers[i];
            ToriObject toriObject = answerObjects[i];

            DeployAnswer(answer, toriObject, isCorrect);

        }
    }

    private void SetAnswerAsTutorial ( Answer answer )
    {
        _ = quizManager.draggingTutorial.SetAnswerAndPlay
            (answer.GetComponent<RectTransform>());
    }

    private void AddWrongAnswers ( List<ToriObject> allObjects, List<ToriObject> answerObjects )
    {
        List<ToriObject> wrongObjects = GetRandomObjects(allObjects, 7);
        answerObjects.AddRange(wrongObjects);
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


    private void DeployAnswer ( Answer answer, ToriObject toriObject, bool isCorrect )
    {
        switch (quizManager.GetSubject().name)
        {
            case "Colors":
                answer.SetColor(toriObject.color);
                break;
            case "Shapes":
                answer.SetImage(toriObject.sprite);
                break;
            case "Animals":
                answer.SetImage(toriObject.sprite);
                break;
        }


        if (isCorrect)
        {
            answer.SetAsCorrect();
            answer.SetTarget(quizManager.questions[0].target);
            if (answer.randomSprite != null)
                answer.randomSprite.SetRandomSprite();
        }

        answer.SetAudioClip(toriObject.clip);
    }


    public void CorrectAnswer ( Answer answer )
    {
        answer.FadeOut();

        if (correctAnswersCounter == 0)
        {
            quizManager.draggingTutorial.StopMoving();
        }

        correctAnswersCounter++;

        if (correctAnswersCounter == quizManager.correctAnswersRequired)
        {
            _ = CelebrateAsync();
        }

        Debug.Log(correctAnswersCounter);
    }

    private async Task CelebrateAsync ()
    {
        quizManager.answersManager.DestroyAllAnswers();

        if (quizManager.clampController != null)
            quizManager.clampController.CloseClamp();

        quizManager.feedbackManager.SetFeedback(FeedbackManager.FeedbackType.Right);
        correctAnswersCounter = 0;
        await Task.Delay(3000);

        // If all objects shown
        if (quizManager.currentObjectIndex + 1 >= quizManager.currentObjects.Count)
        {
            quizManager.CompleteQuiz();
        }
        else
            NextQuestion();
    }

    public void WrongAnswer ()
    {
        quizManager.feedbackManager.SetFeedback(FeedbackManager.FeedbackType.Wrong);
    }

    private void ResetAnswers ()
    {
        quizManager.answersManager.ResetAnswers();
    }

    public void NextQuestion ()
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

    public void FadeInAnswers ()
    {
        quizManager.answersManager.FadeInAnswers();
    }

    public void FadeOutAnswers ()
    {
        quizManager.answersManager.FadeOutAnswers();
    }

}
