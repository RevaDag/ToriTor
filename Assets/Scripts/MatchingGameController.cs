using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchingGameController : MonoBehaviour
{
    [SerializeField] private QuestionAnswerManager answersManager;
    [SerializeField] private Question question;

    private List<Answer> answerList;
    private int currentAnswerIndex = 0;

    private Dictionary<Answer, Question> answerToQuestionMap;


    private void OnEnable ()
    {
        answersManager.OnAnswersManagerReady += OnAnswersManagerReady;
        answersManager.OnCorrectDraggableTarget += OnCorrectAnswer;
    }

    private void OnDisable ()
    {
        answersManager.OnAnswersManagerReady -= OnAnswersManagerReady;
        answersManager.OnCorrectDraggableTarget -= OnCorrectAnswer;

    }

    private void OnAnswersManagerReady ()
    {
        GetAnswers();
        DeployQuestionsAndAnswers();
    }


    private void GetAnswers ()
    {
        answerList = answersManager.GetAnswerList();
    }

    private void DeployQuestionsAndAnswers ()
    {
        answerToQuestionMap = new Dictionary<Answer, Question>();

        Answer lastSelectedAnswer = null;

        if (currentAnswerIndex >= answerList.Count)
        {
            currentAnswerIndex = 0;
        }

        Answer currentAnswer = answerList[currentAnswerIndex];

        if (currentAnswer == lastSelectedAnswer)
        {
            // Select the next answer in the list
            currentAnswerIndex = (currentAnswerIndex + 1) % answerList.Count;
            currentAnswer = answerList[currentAnswerIndex];
        }

        lastSelectedAnswer = currentAnswer;

        SetQuestionAndAssignToAnswer(currentAnswer, question);

        currentAnswerIndex = (currentAnswerIndex + 1) % answerList.Count;
    }


    private void SetQuestionAndAssignToAnswer ( Answer answer, Question question )
    {
        //question.SetQuestion(answer.toriObject);
        answer.draggable?.SetTarget(question.target);
        answerToQuestionMap[answer] = question;
    }

    private void OnCorrectAnswer ()
    {
        answersManager.CorrectAnswer();
    }
}
