using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static QuizManager;

public class FindQuiz : IQuiz
{
    private QuizManager quizManager;

    private List<Question> questions;
    private List<ToriObject> currentObjects;

    private Subject subject;
    private int correctAnswersCounter;

    private int levelNumber = 3;



    public void InitiateQuiz ()
    {
        LoadObjects();
        GetSubject();

        ResetAnswers();

        GetQuestions();
        DeployQuestions(currentObjects);

        InitializeAnswers();
        DeployAnswers();

        quizManager.HideLoadingScreen();

        FadeInAnswers();
    }

    private void LoadObjects ()
    {
        currentObjects = quizManager.LoadObjects(levelNumber);
    }

    public void SetQuizManager ( QuizManager _quizManager )
    {
        this.quizManager = _quizManager;
    }

    private void GetSubject ()
    {
        if (quizManager.quizTester.isTest)
            subject = quizManager.quizTester.subject;
        else
            subject = SubjectsManager.Instance.selectedSubject;
    }


    public void GetQuestions ()
    {
        questions = quizManager.questions;
    }


    public void DeployQuestions ( List<ToriObject> objects )
    {
        for (int i = 0; i < objects.Count; i++)
        {
            DeployQuestion(objects[i], questions[i]);
        }
    }

    public void DeployQuestion ( ToriObject toriObject, Question question )
    {
        switch (subject.name)
        {
            case "Colors":
                question.SetObject(toriObject);
                question.SetImage(toriObject.sprite);
                question.ColorImage(toriObject.color);
                break;
            case "Shapes":
                question.SetImage(toriObject.sprite);
                break;
        }

        question.SetAudioClip(toriObject.clip);
    }

    private void InitializeAnswers ()
    {
        quizManager.answersManager.InitializeAnswers();
    }


    public void DeployAnswers ()
    {
        List<Answer> answers = quizManager.answersManager.GetAnswers();
        if (answers == null || answers.Count < 7)
        {
            Debug.LogError("Insufficient number of answers available.");
            return;
        }

        List<ToriObject> allObjects = new List<ToriObject>(quizManager.GetAllSubjectObjects());

        // Remove the current objects from the list of all objects using object names for comparison
        foreach (ToriObject currentObject in currentObjects)
        {
            allObjects.RemoveAll(obj => obj.objectName == currentObject.objectName);
        }

        // List to store the final answer objects
        List<ToriObject> answerObjects = new List<ToriObject>();

        // Add each correct object (up to 3)
        foreach (ToriObject correctObject in currentObjects)
        {
            if (answerObjects.Count < 3)
            {
                answerObjects.Add(correctObject);
            }
            else
            {
                break;
            }
        }

        // Select and add unique wrong answers from the remaining objects
        while (answerObjects.Count < 7 && allObjects.Count > 0)
        {
            ToriObject randomObject = GetRandomObject(allObjects);
            if (!answerObjects.Contains(randomObject))
            {
                answerObjects.Add(randomObject);
                allObjects.Remove(randomObject);
            }
        }


        // Shuffle the answers list
        ShuffleList(answerObjects);

        for (int i = 0; i < answers.Count; i++)
        {
            Answer answer = answers[i];
            ToriObject toriObject = answerObjects[i];
            bool isCorrect = currentObjects.Contains(toriObject);

            DeployAnswer(answer, toriObject);

            if (isCorrect)
            {
                answer.SetAsCorrect();
            }
        }
    }

    private ToriObject GetRandomObject ( List<ToriObject> objects )
    {
        int randomIndex = UnityEngine.Random.Range(0, objects.Count);
        return objects[randomIndex];
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
        answer.SetImage(toriObject.parallelObjectSprite);
        answer.SetObject(toriObject);

        answer.SetAudioClip(toriObject.parallelObjectClip);
    }


    public void CorrectAnswer ( Answer answer )
    {
        //quizManager.feedbackManager.SetFeedback(FeedbackManager.FeedbackType.Right);
        //quizManager.SetQuestionState(QuestionState.Correct);
        answer.FadeOut();

        _ = ChangeImageToParallelAndShowCheckmark(answer.toriObject);

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
        correctAnswersCounter = 0;
        await Task.Delay(3000);
        quizManager.CompleteQuiz();
    }

    private async Task ChangeImageToParallelAndShowCheckmark ( ToriObject toriObject )
    {
        Question question = GetQuestionWithToriObject(toriObject);

        question.FadeOut();
        question.FlipCard();
        await Task.Delay(1000);

        question.SetAudioClip(toriObject.parallelObjectClip);
        question.SetImage(toriObject.parallelObjectSprite);
        question.ColorImage(Color.white);

        question.FadeIn();
        await Task.Delay(1000);
        ShowCheckMark(toriObject);
    }

    private void ShowCheckMark ( ToriObject toriObject )
    {
        Question question = GetQuestionWithToriObject(toriObject);
        question.FadeInCheckMark();
    }

    private Question GetQuestionWithToriObject ( ToriObject toriObject )
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
        quizManager.ResetUnusedAnswersList();
        quizManager.MoveToNextObject();
        InitiateQuiz();
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
