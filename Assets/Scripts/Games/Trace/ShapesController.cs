using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapesController : MonoBehaviour, IGame
{
    [SerializeField] private List<ShapeHandler> shapeHandlers;
    public FeedbackManager feedbackManager;
    [SerializeField] private GameSummary gameSummary;

    private int currentShapeIndex;
    private bool isClickingOnParallel;

    private void Start ()
    {
        FadeOutShapes();
        SetShapesController();
        gameSummary.SetGameInterface(this);

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
        if (!isClickingOnParallel)
            StartCoroutine(CompleteShapeCorutine());
    }

    private IEnumerator CompleteShapeCorutine ()
    {
        isClickingOnParallel = true;

        yield return new WaitForSeconds(2);
        shapeHandlers[currentShapeIndex].HideParallel();

        currentShapeIndex++;

        if (currentShapeIndex >= shapeHandlers.Count)
            CompleteGame();
        else
            NextShape();

        isClickingOnParallel = false;
    }

    private void NextShape ()
    {
        shapeHandlers[currentShapeIndex].FadeIn();
    }

    private void CompleteGame ()
    {
        gameSummary.ShowSummary();

        if (GameManager.Instance != null)
            GameManager.Instance.CompleteLevelAndProgressToNextLevel();
    }

    public void ResetGame ()
    {
        FadeOutShapes();
        ResetAllShapes();
        currentShapeIndex = 0;
        //SetShapesController();

        shapeHandlers[0].FadeIn();
    }

    private void ResetAllShapes ()
    {
        foreach (ShapeHandler handler in shapeHandlers)
        {
            handler.ResetShape();
        }
    }



}
