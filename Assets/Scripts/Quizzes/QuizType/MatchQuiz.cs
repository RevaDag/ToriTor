using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static SubjectsManager;

public class MatchQuiz : IQuiz
{
    private Question currentQuestion;
    private List<Answer> answers = new List<Answer>();
    private QuizManager quizManager;
    private ToriObject currentToriObject;
    private Subject subject;

    public void InitiateQuiz ()
    {
        LoadCurrentQuestion();
        DeployAnswers();
        FadeInAnswers();
    }

    public void SetQuizManager ( QuizManager _quizManager )
    {
        this.quizManager = _quizManager;
    }

    public void SetSubject ( Subject _subject )
    {
        subject = _subject;
    }

    public void SetQuestion ( Question question )
    {
        currentQuestion = question;
    }

    public void SetAnswers ( List<Answer> _answers )
    {
        answers = _answers;
    }

    public void LoadCurrentQuestion ()
    {
        currentToriObject = quizManager.GetCurrentObject();
        DeployQuestion(currentToriObject);
    }

    public void DeployQuestion ( ToriObject toriObject )
    {
        currentQuestion.SetImage(toriObject.parallelObjectSprite);
        currentQuestion.SetAudioClip(toriObject.parallelObjectClip);
    }


    public void NextQuestion ()
    {
        LoadCurrentQuestion();
    }

    public void DeployAnswers ()
    {
        if (answers == null || answers.Count < 3)
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
        correctAnswer.SetTarget(currentQuestion.target);

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
        answer.SetImage(toriObject);
        answer.SetColor(toriObject);
        answer.SetAudioClip(toriObject);
    }

    public void AnswerClicked ( bool isCorrect )
    {

    }


    public void CorrectAnswer ()
    {
        quizManager.feedbackManager.SendFeedback(0);
        FadeOutAnswers();
        quizManager.stepper.activateNextStep();
    }

    public void WrongAnswer ()
    {
        quizManager.feedbackManager.SendFeedback(1);
    }

    private void ResetAnswers ()
    {
        foreach (Answer answer in answers)
        {
            answer.ResetAnswer();
        }
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

    public void FadeInAnswers ()
    {
        foreach (Answer answer in answers)
        {
            answer.FadeIn();
        }
    }

    public void FadeOutAnswers ()
    {
        foreach (Answer answer in answers)
        {
            answer.FadeOut();
        }
    }

}
