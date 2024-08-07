using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MemoryGame : MonoBehaviour, IGame
{
    [SerializeField] private QuizSummary quizSummary;

    [SerializeField] private List<MemoryCard> cards;
    private MemoryCard firstRevealed;
    private MemoryCard secondRevealed;

    private Subject subject;
    private List<ToriObject> currentObjects;

    private int matchedCouples;

    [Header("Test")]
    [SerializeField] private QuizTester quizTester;

    private void Start ()
    {
        GetSubject();
        LoadObjects();
        DeployCards();
        quizSummary.SetGameInterface(this);

        if (LoadingScreen.Instance != null)
            LoadingScreen.Instance.HideLoadingScreen();

        StartCoroutine(RevealAndHide());
    }


    private void LoadObjects ()
    {
        if (quizTester.isTest)
            currentObjects = quizTester.selectedObjects;
        else
            currentObjects = SubjectsManager.Instance.GetObjectsByListNumber(quizSummary.levelNumber);

    }

    private void GetSubject ()
    {
        if (quizTester.isTest)
            subject = quizTester.subject;
        else
            subject = SubjectsManager.Instance.selectedSubject;
    }

    private void DeployCards ()
    {
        // Check if we have enough cards
        if (cards.Count < currentObjects.Count * 2)
        {
            Debug.LogError("Not enough cards in the scene to match the required pairs.");
            return;
        }

        // Duplicate the objects to create pairs
        List<ToriObject> pairedObjects = new List<ToriObject>(currentObjects);
        pairedObjects.AddRange(currentObjects);

        // Shuffle the paired objects
        Shuffle(pairedObjects);

        for (int i = 0; i < pairedObjects.Count; i++)
        {
            cards[i].DeployCard(pairedObjects[i]);
        }
    }

    private void Shuffle<T> ( List<T> list )
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void CardRevealed ( MemoryCard card )
    {
        if (firstRevealed == null)
        {
            firstRevealed = card;
        }
        else
        {
            secondRevealed = card;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch ()
    {
        if (firstRevealed.sticker.toriObject == secondRevealed.sticker.toriObject)
        {
            yield return new WaitForSeconds(2.0f);
            StartCoroutine(MatchCards());
        }
        else
        {
            yield return new WaitForSeconds(3.0f);
            firstRevealed.HideObject();
            secondRevealed.HideObject();
        }

        firstRevealed = null;
        secondRevealed = null;
    }

    private IEnumerator MatchCards ()
    {
        firstRevealed.ShowParallel();
        secondRevealed.ShowParallel();

        matchedCouples++;

        yield return new WaitForSeconds(2.0f);


        if (matchedCouples >= 3)
            CompleteGame();
    }

    private void CompleteGame ()
    {
        quizSummary.ShowSummary();

        GameManager.Instance.CompleteLevelAndProgressToNextLevel(quizSummary.levelNumber);
    }

    public bool CanRevealCard ()
    {
        return secondRevealed == null;
    }

    private void HideAllCards ()
    {
        foreach (MemoryCard memoryCard in cards)
        {
            memoryCard.HideObject();
        }
    }

    private void RevealAllCards ()
    {
        foreach (MemoryCard memoryCard in cards)
        {
            memoryCard.RevealObject();
        }
    }

    public void ResetGame ()
    {
        matchedCouples = 0;
        HideAllCards();
        ResetAllCards();
        DeployCards();
        StartCoroutine(RevealAndHide());
    }

    private void ResetAllCards ()
    {
        foreach (MemoryCard memoryCard in cards)
        {
            memoryCard.ResetCard();
        }
    }

    public IEnumerator RevealAndHide ()
    {
        RevealAllCards();

        yield return new WaitForSeconds(4f);

        HideAllCards();
    }
}
