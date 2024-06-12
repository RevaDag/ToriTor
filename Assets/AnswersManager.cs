using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswersManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private QuestionBubble questionBubble;
    [SerializeField] private List<Answer> answers;
    [SerializeField] private Transform answersParent;

    private int currentToriObjectIndex;
    private List<ToriObject> toriObjects;

    private void Start ()
    {
        LoadLevelObjects();
        SetAnswers();
    }

    private void LoadLevelObjects ()
    {
        toriObjects?.Clear();
        toriObjects = GameManager.Instance.currentLevelObjects;
    }

    public void SetAnswers ()
    {
        List<ToriObject> availableToriObjects = new List<ToriObject>(toriObjects);

        for (int i = 0; i < answers.Count; i++)
        {
            answers[i].ResetAnswer();

            if (i < availableToriObjects.Count)
            {
                ToriObject toriObject = availableToriObjects[i];

                answers[i].SetAnswer(toriObject);
                answers[i].SetAnswersManager(this);
            }
        }

        Answer correctAnswer = answers[currentToriObjectIndex % answers.Count];
        correctAnswer.SetAsCorrect();

        questionBubble.SetQuestionBubble(correctAnswer.toriObject);

    }

    public void NextAnswer ()
    {
        currentToriObjectIndex++;
        levelManager.NextStep();
        SetAnswers();
    }
}
