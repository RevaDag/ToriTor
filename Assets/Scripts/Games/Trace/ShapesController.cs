using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapesController : MonoBehaviour
{
    [SerializeField] private List<ShapeHandler> shapeHandlers;
    public FeedbackManager feedbackManager;
    [SerializeField] private GameSummary quizSummary;

    private int currentShapeIndex;

    private void Start ()
    {
        FadeOutShapes();
        SetShapesController();

        if (LoadingScreen.Instance != null)
            LoadingScreen.Instance.HideLoadingScreen();

        shapeHandlers[0].FadeIn();
    }

    private void SetShapesController ()
    {
        foreach (ShapeHandler handler in shapeHandlers)
        {
            handler.SetShapesController(this);
        }
    }

    private void FadeOutShapes ()
    {
        foreach (ShapeHandler handler in shapeHandlers)
        {
            handler.FadeOut();
        }
    }

    public void CompleteShape ()
    {
        StartCoroutine(CompleteShapeCorutine());
    }

    private IEnumerator CompleteShapeCorutine ()
    {
        yield return new WaitForSeconds(2);
        shapeHandlers[currentShapeIndex].HideParallel();

        currentShapeIndex++;

        if (currentShapeIndex >= shapeHandlers.Count)
            CompleteGame();
        else
            NextShape();
    }

    private void NextShape ()
    {
        shapeHandlers[currentShapeIndex].FadeIn();
    }

    private void CompleteGame ()
    {
        quizSummary.ShowSummary();

        if (GameManager.Instance != null)
            GameManager.Instance.CompleteLevelAndProgressToNextLevel(quizSummary.levelNumber);
    }



}
