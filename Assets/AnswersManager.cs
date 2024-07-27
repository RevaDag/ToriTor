using System.Collections.Generic;
using UnityEngine;

public class AnswersManager : MonoBehaviour
{
    [SerializeField] private List<Answer> answers;
    [SerializeField] private Answer answerPrefab;
    [SerializeField] private Canvas canvas;
    [SerializeField] private int desiredAnswerCount = 5;

    private List<Answer> activeAnswers = new List<Answer>();
    private List<Answer> unusedAnswers = new List<Answer>();

    public List<Answer> InitializeAnswers ()
    {
        activeAnswers = new List<Answer>(answers);
        ResetUnusedAnswersList();

        return activeAnswers;
    }

    public List<Answer> GetAnswers ()
    {
        return answers;
    }

    public Answer GetUnusedAnswer ()
    {
        if (unusedAnswers.Count == 0)
        {
            ResetUnusedAnswersList();
        }

        int randomIndex = Random.Range(0, unusedAnswers.Count);
        Answer answer = unusedAnswers[randomIndex];
        unusedAnswers.RemoveAt(randomIndex);
        return answer;
    }

    public void ResetAnswers ()
    {
        foreach (Answer answer in answers)
        {
            answer.ResetAnswer();
        }
    }

    public void ResetUnusedAnswersList ()
    {
        unusedAnswers = new List<Answer>(activeAnswers);
    }

    public void DestroyAnswer ( Answer answerToDestroy )
    {
        if (answerToDestroy != null && activeAnswers.Contains(answerToDestroy))
        {
            activeAnswers.Remove(answerToDestroy);
            unusedAnswers.Remove(answerToDestroy);
            Destroy(answerToDestroy.gameObject);
            MaintainAnswerCount();
        }
    }

    private void MaintainAnswerCount ()
    {
        while (activeAnswers.Count < desiredAnswerCount)
        {
            Answer newAnswer = InstantiateAnswer();
            if (newAnswer != null)
            {
                activeAnswers.Add(newAnswer);
                unusedAnswers.Add(newAnswer);
            }
        }
    }

    private Answer InstantiateAnswer ()
    {
        if (answerPrefab == null)
        {
            Debug.LogError("Answer prefab is not assigned!");
            return null;
        }

        Answer answerInstance = Instantiate(answerPrefab, canvas.transform);
        RectTransform rectTransform = answerInstance.GetComponent<RectTransform>();

        if (rectTransform == null)
        {
            Debug.LogError("RectTransform component not found on the answer prefab!");
            return answerInstance;
        }

        Vector2 randomPosition = GetRandomPositionOnScreen(rectTransform);
        rectTransform.anchorMin = rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = randomPosition;

        return answerInstance;
    }

    private Vector2 GetRandomPositionOnScreen ( RectTransform rectTransform )
    {
        float canvasWidth = canvas.GetComponent<RectTransform>().rect.width;
        float canvasHeight = canvas.GetComponent<RectTransform>().rect.height;

        float prefabWidth = rectTransform.rect.width;
        float prefabHeight = rectTransform.rect.height;

        float randomX = Random.Range(-canvasWidth / 2 + prefabWidth / 2, canvasWidth / 2 - prefabWidth / 2);
        float randomY = Random.Range(-canvasHeight / 2 + prefabHeight / 2, canvasHeight / 2 - prefabHeight / 2);

        return new Vector2(randomX, randomY);
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

    public List<Answer> GetActiveAnswers ()
    {
        return new List<Answer>(activeAnswers);
    }
}