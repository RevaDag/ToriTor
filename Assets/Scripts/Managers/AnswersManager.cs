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
    public Canvas canvas;
    public RectTransform pearlsParent;
    [SerializeField] private RectTransform target;

    private List<Answer> activeAnswers = new List<Answer>();
    private List<Answer> unusedAnswers = new List<Answer>();

    public Answer currentAnswer { get; private set; }

    public int numberOfAnswers;

    public List<Answer> InitializeAnswers ()
    {
        activeAnswers = new List<Answer>(answers);

        foreach (var answer in activeAnswers)
        {
            answer.Initialize();
        }

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

    public async Task InstantiateAnswersAsync ( int desiredAnswerCount = -1 )
    {
        if (desiredAnswerCount < 0)
            desiredAnswerCount = numberOfAnswers;

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
        // Instantiate the answer prefab as a child of the pearlsParent
        Answer answerInstance = Instantiate(answerPrefab, pearlsParent);

        // Set the AnswersManager reference and initialize the answer
        answerInstance.SetAnswersManager(this);
        answerInstance.Initialize();

        // Get the RectTransform component of the instantiated answer
        RectTransform rectTransform = answerInstance.GetComponent<RectTransform>();

        // Ensure the UI element is correctly aligned by setting the local position, anchored position, and scale
        rectTransform.localPosition = Vector3.zero;       // Set local position to (0, 0, 0)
        rectTransform.anchoredPosition = Vector2.zero;    // Set anchored position to (0, 0)
        //rectTransform.localScale = Vector3.one;           // Reset the scale in case it's affected by the parent

        // Optionally, reset the rotation if it's not supposed to have any
        rectTransform.localRotation = Quaternion.identity;

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