using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchingGameController : MonoBehaviour
{
    [SerializeField] private AnswersManager answersManager;
    [SerializeField] private List<Question> questionList;

    private List<Answer> answerList;

    private Dictionary<Answer, Question> answerToQuestionMap;


    private void OnEnable ()
    {
        answersManager.OnAnswersManagerReady += OnAnswersManagerReady;
    }

    private void OnDisable ()
    {
        answersManager.OnAnswersManagerReady -= OnAnswersManagerReady;
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
        // Shuffle the answers list
        System.Random rng = new System.Random();
        int n = answerList.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Answer value = answerList[k];
            answerList[k] = answerList[n];
            answerList[n] = value;
        }

        // Initialize the dictionary
        answerToQuestionMap = new Dictionary<Answer, Question>();

        // Deploy each answer to a question
        for (int i = 0; i < questionList.Count; i++)
        {
            if (i < answerList.Count)
            {
                Answer answer = answerList[i];
                Question question = questionList[i];

                //DeployAnswer(toriObject, answer);
                SetQuestionAndAssignToAnswer(answer, question);
            }
        }
    }


    private void SetQuestionAndAssignToAnswer ( Answer answer, Question question )
    {
        question.SetQuestion(answer.toriObject);
        answer.draggable?.SetTarget(question.target);
        answerToQuestionMap[answer] = question;
    }
}
