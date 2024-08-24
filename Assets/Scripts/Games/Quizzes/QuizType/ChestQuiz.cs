using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static QuizManager;

public class ChestQuiz : IQuiz
{
    private QuizManager quizManager;
    private ToriObject currentToriObject;
    private Animator lidAnimator;
    private Animator parallelObjectAnimator;
    private Sticker parallelObjectSticker;

    private int correctAnswersCounter;

    public void InitiateQuiz ()
    {
        quizManager.LoadLevelObjects();
        LoadCurrentQuestion();
        InitializeAnswers();
        DeployAnswers();

        quizManager.HideLoadingScreen();

        FadeInAnswers();
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
        lidAnimator = quizManager.chestLidAnimator;
        parallelObjectAnimator = quizManager.parallelObjectAnimator;
        parallelObjectSticker = parallelObjectAnimator.GetComponent<Sticker>();
    }



    public void LoadCurrentQuestion ()
    {
        currentToriObject = quizManager.GetCurrentObject();
        lidAnimator.SetBool("isOpen", false);

        DeployQuestion(quizManager.questions[0], currentToriObject);
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


        }
    }

    private void InitializeAnswers ()
    {
        quizManager.answersManager.InitializeAnswers();
    }

    private void DeployParallelObjectSticker ( ToriObject toriObject )
    {
        parallelObjectSticker.SetImage(toriObject.parallelObjectSprite);
        parallelObjectSticker.SetAudio(toriObject.parallelObjectClip);
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
        correctAnswer.SetTarget(quizManager.questions[0].target);

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

        }

        answer.SetAudioClip(toriObject.clip);
    }


    public void CorrectAnswer ( Answer answer )
    {
        answer.PlayClip();

        correctAnswersCounter++;

        lidAnimator.SetBool("isOpen", true);
        ParallelObjectAnimation(true);
        FadeOutAnswers();
    }

    public void ParallelObjectAnimation ( bool isOut )
    {
        DeployParallelObjectSticker(currentToriObject);

        parallelObjectAnimator.SetBool("isOut", isOut);
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

            ParallelObjectAnimation(false);
            quizManager.ResetUnusedAnswersList();
            quizManager.MoveToNextObject();
            InitiateQuiz();
        }

    }


    public void CompleteQuiz ()
    {
        ParallelObjectAnimation(false);
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
