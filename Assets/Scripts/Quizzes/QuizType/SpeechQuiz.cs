using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static QuizManager;
using static SubjectsManager;

public class SpeechQuiz : IQuiz
{
    private Question currentQuestion;
    private QuizManager quizManager;
    private ToriObject currentToriObject;

    public void InitiateQuiz ()
    {
        LoadCurrentQuestion();
        InitializeAnswers();
        DeployAnswers();
        FadeInAnswers();
    }

    public void SetQuizManager ( QuizManager _quizManager )
    {
        this.quizManager = _quizManager;
    }

    public void SetSubject ( Subject subject )
    {

    }

    public void SetQuestions ( Question question )
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
        currentQuestion.SetAudioClip(toriObject.clip);
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
        answer.SetColor(toriObject.color);
        answer.SetAudioClip(toriObject.clip);
    }


    public void CorrectAnswer ( Answer answer )
    {
        ResetAnswers();
        quizManager.feedbackManager.SetFeedback(FeedbackManager.FeedbackType.Right);
        quizManager.SetQuestionState(QuestionState.Correct);
        FadeOutAnswers();
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
        quizManager.answersManager.FadeInAnswers();
    }

}
