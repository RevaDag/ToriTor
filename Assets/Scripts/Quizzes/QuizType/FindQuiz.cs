using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static QuizManager;

public class FindQuiz : IQuiz
{
    private Question currentQuestion;
    private QuizManager quizManager;
    private ToriObject currentToriObject;

    private Subject subject;
    private int correctAnswersCounter;

    private int levelNumber = 3;



    public void InitiateQuiz ()
    {
        LoadObjects();
        GetSubjectFromManager();

        quizManager.SetAnswersQuizManager();
        ResetAnswers();
        LoadCurrentQuestion();
        InitializeAnswers();
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
                currentQuestion.SetImage(toriObject.sprite);
                currentQuestion.ColorImage(toriObject.color);
                break;
            case "Shapes":
                currentQuestion.SetImage(toriObject.sprite);
                break;
        }

        currentQuestion.SetAudioClip(toriObject.clip);
    }

    private void InitializeAnswers ()
    {
        quizManager.answersManager.InitializeAnswers();
    }


    public void AnswerClicked ( bool isCorrect )
    {
        if (isCorrect)
        {
            quizManager.CorrectAnswer();
            Debug.Log("CORRECT!");
        }
        else
        {
            quizManager.WrongAnswer();
            Debug.Log("WRONG!");
        }
    }

    public void NextQuestion ()
    {
        LoadCurrentQuestion();

    }

    public void DeployAnswers ()
    {
        List<Answer> answers = quizManager.answersManager.GetAnswers();


        if (answers == null)
        {
            Debug.LogError("Insufficient number of answers available.");
            return;
        }

        ToriObject correctObject = quizManager.GetCurrentObject();
        List<ToriObject> wrongObjects = quizManager.GetRandomObjects(2, correctObject);

        // Shuffle the answers list to randomize the position of the correct answer
        List<Answer> shuffledAnswers = new List<Answer>(answers);
        ShuffleList(shuffledAnswers);

        // Deploy the correct answer to a random position
        int correctAnswerIndex = Random.Range(0, shuffledAnswers.Count);
        Answer correctAnswer = shuffledAnswers[correctAnswerIndex];
        DeployAnswer(correctAnswer, correctObject);
        correctAnswer.SetAsCorrect();

        // Deploy wrong answers to the remaining positions
        int wrongObjectIndex = 0;
        for (int i = 0; i < shuffledAnswers.Count; i++)
        {
            if (i != correctAnswerIndex)
            {
                DeployAnswer(shuffledAnswers[i], wrongObjects[wrongObjectIndex]);
                wrongObjectIndex++;
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
        answer.SetImage(toriObject.parallelObjectSprite);

        answer.SetAudioClip(toriObject.parallelObjectClip);
    }


    public void CorrectAnswer ()
    {
        ResetAnswers();
        quizManager.feedbackManager.SendFeedback(0);
        quizManager.SetQuestionState(QuestionState.Correct);
        FadeOutAnswers();
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
