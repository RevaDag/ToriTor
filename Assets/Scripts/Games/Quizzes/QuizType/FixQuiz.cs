using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FixQuiz : IQuiz
{
    private QuizManager quizManager;

    private List<ToriObject> currentObjects;
    private int correctAnswersCounter;

    public void InitiateQuiz ()
    {
        LoadObjects();

        quizManager.HideLoadingScreen();

        DeployQuestions(currentObjects);

        InitializeAnswers();
        DeployAnswers();

        quizManager.HideLoadingScreen();

        FadeInAnswers();
    }


    public void ResetQuiz ()
    {
        ResetAnswers();
        InitiateQuiz();
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
        question.SetObject(toriObject);
        question.SetImages(toriObject.sprite);
        question.SetAudioClip(toriObject.clip);
    }

    private void InitializeAnswers ()
    {
        quizManager.answersManager.InitializeAnswers();
    }


    public void DeployAnswers ()
    {
        List<Answer> answers = quizManager.answersManager.GetAnswers();
        if (answers == null || answers.Count < 3)
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
        while (answerObjects.Count < 3)
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

                Question matchedQuestion = quizManager.GetQuestionWithToriObject(toriObject);

                answer.SetTarget(matchedQuestion.target);
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
        answer.SetImage(toriObject.sprite);
        answer.SetObject(toriObject);

        answer.SetAudioClip(toriObject.clip);
    }


    public void CorrectAnswer ( Answer answer )
    {
        quizManager.feedbackManager.SetFeedback(FeedbackManager.FeedbackType.Right);
        answer.PlayClip();

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
        await Task.Delay(2000);
        quizManager.CompleteQuiz();
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
