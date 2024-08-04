using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static QuizManager;

public class CatchQuiz : IQuiz
{
    private Question question;
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

        await InstantiateAnswers();

        ResetAnswers();

        GetQuestion();
        LoadCurrentQuestion();


        DeployAnswers();

        quizManager.clampController.OpenClamp();
        FadeInAnswers();
    }

    private async Task InstantiateAnswers ()
    {
        // If this is a tutorial 
        if (quizManager.currentObjectIndex == 0)
            await quizManager.answersManager.InstantiateAnswersAsync(3);
        else
            await quizManager.answersManager.InstantiateAnswersAsync(8);
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
        if (quizManager.quizTester.isTest)
            subject = quizManager.quizTester.subject;
        else
            subject = SubjectsManager.Instance.selectedSubject;
    }

    public void GetQuestion ()
    {
        question = quizManager.questions[0];
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
                question.ColorImage(toriObject.color);
                break;
            case "Shapes":
                question.SetImage(toriObject.sprite);
                break;
        }

        question.SetAudioClip(toriObject.clip);
        question.PlayAudioSouce();
    }


    public void AnswerClicked ( bool isCorrect )
    {

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

        quizManager.PlayClip(quizManager.bubbles);

        for (int i = 0; i < shuffledAnswers.Count; i++)
        {
            bool isCorrect = i < 3;
            Answer answer = shuffledAnswers[i];
            ToriObject toriObject = answerObjects[i];

            DeployAnswer(answer, toriObject);

            if (isCorrect)
            {
                answer.SetAsCorrect();
                answer.SetTarget(question.target);
            }
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


    public void CorrectAnswer ( Answer answer )
    {
        answer.FadeOut();

        if (correctAnswersCounter == 0)
        {
            quizManager.draggingTutorial.StopMoving();
        }

        if (correctAnswersCounter == 2)
        {
            _ = CelebrateAsync();
        }
        else
        {
            correctAnswersCounter++;
        }

    }

    private async Task CelebrateAsync ()
    {
        quizManager.answersManager.DestroyAllAnswers();
        quizManager.clampController.CloseClamp();
        quizManager.SetQuestionState(QuestionState.Correct);
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
