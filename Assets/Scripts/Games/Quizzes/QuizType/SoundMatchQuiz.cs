using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static QuizManager;

public class SoundMatchQuiz : IQuiz
{
    private QuizManager quizManager;
    private ToriObject currentToriObject;
    private Question currentQuestion;
    private int correctAnswersCounter;

    public void InitiateQuiz ()
    {
        quizManager.LoadLevelObjects();
        LoadCurrentQuestion();
        InitializeAnswers();
        DeployAnswers();

        quizManager.HideLoadingScreen();

        FadeInAnswers();

        _ = PlayQuestionAsync();
    }

    private async Task PlayQuestionAsync ()
    {
        await Task.Delay(1000);
        currentQuestion.PlayAudioSouce();
        currentQuestion.animator.SetTrigger("Expand");
    }


    public void ResetQuiz ()
    {
        correctAnswersCounter = 0;
        ResetAnswers();
        InitiateQuiz();
    }

    public void SetQuizManager ( QuizManager _quizManager )
    {
        this.quizManager = _quizManager;
    }



    public void LoadCurrentQuestion ()
    {
        currentToriObject = quizManager.GetCurrentObject();

        if (currentQuestion == null)
            currentQuestion = quizManager.questions[0];

        DeployQuestion(currentQuestion, currentToriObject);
    }

    public void DeployQuestion ( Question question, ToriObject toriObject )
    {
        switch (quizManager.GetSubject().name)
        {
            case "Colors":
                question.ColorImage(toriObject.color);
                break;
            case "Shapes":
                question.SetImages(toriObject.sprite);
                break;
            case "Animals":
                question.ColorImage(Color.black);
                question.SetImages(toriObject.sprite);
                question.SetAudioClip(toriObject.objectSoundClip);

                break;


        }
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
                answer.SetAudioClip(toriObject.clip);
                break;

        }

        answer.SetAudioClip(toriObject.clip);
    }


    public void CorrectAnswer ( Answer answer )
    {
        _ = CorrectAnswerAsync(answer);
    }

    private async Task CorrectAnswerAsync ( Answer answer )
    {
        answer.PlayClip();
        currentQuestion.ColorImage(Color.white);
        currentQuestion.animator.SetTrigger("Expand");

        await Task.Delay(1000);

        quizManager.SetQuestionState(QuestionState.Correct);
        correctAnswersCounter++;

        currentQuestion.FadeOut();
        FadeOutAnswers();

        await Task.Delay(1000);

        NextQuestion();

        currentQuestion.FadeIn();
        FadeInAnswers();
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
        if (correctAnswersCounter >= 3)
            CompleteQuiz();
        else
        {
            ResetAnswers();

            quizManager.ResetUnusedAnswersList();
            quizManager.MoveToNextObject();
            InitiateQuiz();
        }

    }


    public void CompleteQuiz ()
    {
        quizManager.CompleteQuiz();
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
