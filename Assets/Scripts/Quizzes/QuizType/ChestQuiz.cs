using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestQuiz : IQuiz
{
    private Question currentQuestion;
    private List<Answer> answers = new List<Answer>();
    private QuizManager quizManager;
    private ToriObject currentToriObject;
    private Animator lidAnimator;
    private Animator parallelObjectAnimator;
    private Sticker parallelObjectSticker;

    private Subject subject;
    private Color transparent;



    public void InitiateQuiz ()
    {
        ResetAnswers();
        LoadCurrentQuestion();
        DeployAnswers();
        FadeInAnswers();
    }

    public void SetQuizManager ( QuizManager _quizManager )
    {
        this.quizManager = _quizManager;
        lidAnimator = quizManager.chestLidAnimator;
        parallelObjectAnimator = quizManager.parallelObjectAnimator;
        parallelObjectSticker = parallelObjectAnimator.GetComponent<Sticker>();

        transparent = parallelObjectSticker.GetColor();
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
        lidAnimator.SetBool("isOpen", false);

        DeployQuestion(currentToriObject);
    }

    public void DeployQuestion ( ToriObject toriObject )
    {
        switch (subject.name)
        {
            case "Colors":
                currentQuestion.ColorImage(toriObject.color);
                break;
            case "Shapes":
                currentQuestion.SetImage(toriObject.sprite);
                break;


        }
    }

    private void DeployParallelObjectSticker ( ToriObject toriObject )
    {
        parallelObjectSticker.SetImage(toriObject.parallelObjectSprite);
        parallelObjectSticker.SetAudio(toriObject.parallelObjectClip);
    }

    public void AnswerClicked ( bool isCorrect )
    {

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
        switch (subject.name)
        {
            case "Colors":
                answer.SetColor(toriObject);
                break;
            case "Shapes":
                answer.SetImage(toriObject);
                break;

        }

        answer.SetAudioClip(toriObject);
    }


    public void CorrectAnswer ()
    {
        quizManager.feedbackManager.SendFeedback(0);
        lidAnimator.SetBool("isOpen", true);

        ParallelObjectAnimation(true);
        FadeOutAnswers();
        quizManager.stepper.activateNextStep();
    }

    public void ParallelObjectAnimation ( bool isOut )
    {
        DeployParallelObjectSticker(currentToriObject);

        parallelObjectAnimator.SetBool("isOut", isOut);
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

        ParallelObjectAnimation(false);
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
        ParallelObjectAnimation(false);

        if (quizManager.isTest)
            quizManager.quizSummary.SetObjects(quizManager.quizTester.subject.toriObjects);
        else
            quizManager.quizSummary.SetObjects(quizManager.selectedObjects);

        quizManager.quizSummary.InstantiateStickers();
        GameManager.Instance.SaveProgress();
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
