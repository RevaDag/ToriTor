using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AnswersManager : MonoBehaviour
{
    public QuizManager quizManager;
    [SerializeField] private List<Answer> answers;
    [SerializeField] private Answer answerPrefab;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform pearlsParent;
    [SerializeField] private RectTransform target;

    private List<Answer> activeAnswers = new List<Answer>();
    private List<Answer> unusedAnswers = new List<Answer>();

    public Answer currentAnswer { get; private set; }

    public List<Answer> InitializeAnswers ()
    {
        activeAnswers = new List<Answer>(answers);
        ResetUnusedAnswersList();

        return activeAnswers;
    }

    public void SetCurrentAnswer ( Answer _currentAnswer )
    {
        currentAnswer = _currentAnswer;
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
            answers.Remove(answerToDestroy);
            Destroy(answerToDestroy.gameObject);
        }
    }

    public void DestroyAllAnswers ()
    {
        List<Answer> answersToDestroy = new List<Answer>(activeAnswers);

        foreach (Answer answer in answersToDestroy)
        {
            answer.FadeOut();
        }

        foreach (Answer answer in answersToDestroy)
        {
            DestroyAnswer(answer);
        }
    }

    public async Task InstantiateAnswersAsync ( int desiredAnswerCount )
    {
        while (activeAnswers.Count < desiredAnswerCount)
        {
            Answer newAnswer = InstantiateAnswer();
            if (newAnswer != null)
            {
                answers.Add(newAnswer);
                activeAnswers.Add(newAnswer);
                unusedAnswers.Add(newAnswer);
            }
            await Task.Delay(100); // Small delay between instantiations, similar to yield return new WaitForSeconds(0.1f)
        }

        // Force a rebuild of the Canvas
        LayoutRebuilder.ForceRebuildLayoutImmediate(canvas.GetComponent<RectTransform>());
    }

    private Answer InstantiateAnswer ()
    {
        Answer answerInstance = Instantiate(answerPrefab, pearlsParent);
        answerInstance.SetAnswersManager(this);

        RectTransform rectTransform = answerInstance.GetComponent<RectTransform>();

        rectTransform.position = target.position;

        return answerInstance;
    }

    #region Fade In & Out

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

    public void FadeOutAnswer ( Answer answer )
    { answer.FadeOut(); }

    public void FadeInAnswer ( Answer answer )
    { answer.FadeIn(); }

    #endregion

    public List<Answer> GetActiveAnswers ()
    {
        return new List<Answer>(activeAnswers);
    }
}