using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static QuizManager;

public class FindQuiz : IQuiz
{
    private QuizManager quizManager;

    private List<ToriObject> currentObjects;

    private int correctAnswersCounter;

    public void InitiateQuiz ()
    {
        LoadObjects();

        // ResetAnswers();

        DeployQuestions(currentObjects);

        InitializeAnswers();
        DeployAnswers();

        quizManager.HideLoadingScreen();

        FadeInAnswers();
    }


    public void ResetQuiz ()
    {
        ResetCards();
        ResetAnswers();
        InitiateQuiz();
    }

    private void ResetCards ()
    {
        List<Question> questions = quizManager.questions;
        foreach (Question question in questions)
        {
            question.findCard.ResetCard();
        }
    }

    private void LoadObjects ()
    {
        currentObjects = quizManager.LoadLevelObjects();
    }

    public void SetQuizManager ( QuizManager _quizManager )
    {
        this.quizManager = _quizManager;
    }



    public void DeployQuestions ( List<ToriObject> objects )
    {
        for (int i = 0; i < objects.Count; i++)
        {
            DeployQuestion(objects[i], quizManager.questions[i]);
        }
    }

    public void DeployQuestion ( ToriObject toriObject, Question question )
    {
        switch (quizManager.GetSubject().name)
        {
            case "Colors":
                question.SetObject(toriObject);
                question.SetImages(toriObject.sprite);
                question.ColorImage(toriObject.color);
                break;
            case "Shapes":
                question.SetImages(toriObject.sprite);
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
        question.findCard.FlipCard();
        await Task.Delay(1000);

        question.SetAudioClip(toriObject.parallelObjectClip);
        question.SetImages(toriObject.parallelObjectSprite);
        question.ColorImage(Color.white);

        question.FadeIn();
        await Task.Delay(1000);
        ShowCheckMark(toriObject);
    }

    private void ShowCheckMark ( ToriObject toriObject )
    {
        Question question = GetQuestionWithToriObject(toriObject);
        question.findCard.FadeInCheckMark();
    }

    private Question GetQuestionWithToriObject ( ToriObject toriObject )
    {
        foreach (Question question in quizManager.questions)
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


    public void FadeInAnswers ()
    {
        quizManager.answersManager.FadeInAnswers();
    }

    public void FadeOutAnswers ()
    {
        quizManager.answersManager.FadeOutAnswers();
    }

}
