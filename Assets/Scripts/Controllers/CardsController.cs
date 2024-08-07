using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CardsController : MonoBehaviour, IGame
{
    [SerializeField] private Sticker sticker;
    [SerializeField] private Animator cardAnimator;
    [SerializeField] private QuizSummary quizSummary;
    [SerializeField] private Fader tutorialFader;

    private bool isParallel;

    private ToriObject currentObject;
    private int currentObjectIndex;
    private int maxObjects = 3;

    private bool clicked;

    private void Start ()
    {
        LoadingScreen.Instance.HideLoadingScreen();
        quizSummary.SetGameInterface(this);
        SetCard();
        tutorialFader.FadeIn();
    }

    private void SetCard ()
    {
        isParallel = false;
        cardAnimator.SetTrigger("Flip");
        currentObject = SubjectsManager.Instance.toriObjects1[currentObjectIndex];
        sticker.SetImage(currentObject.sprite);
        sticker.SetColor(currentObject.color);
        sticker.SetAudio(currentObject.clip);
    }

    public async void OnClick ()
    {
        if (clicked) return;

        clicked = true;
        sticker.PlayAudio();
        await Task.Delay(1000);

        if (isParallel)
        {
            NextObject();
        }
        else
        {
            ShowParallel();
        }

        await Task.Delay(1000);
        clicked = false;
    }

    public void ShowParallel ()
    {
        isParallel = true;
        cardAnimator.SetTrigger("Flip");
        sticker.SetImage(currentObject.parallelObjectSprite);
        sticker.SetColor(Color.white);
        sticker.SetAudio(currentObject.parallelObjectClip);
    }

    public void NextObject ()
    {
        if (currentObjectIndex == 0)
            tutorialFader.FadeOut();

        if (currentObjectIndex < maxObjects - 1)
        {
            currentObjectIndex++;
            SetCard();
        }
        else
        {
            quizSummary.ShowSummary();
            GameManager.Instance.CompleteLevelAndProgressToNextLevel(quizSummary.levelNumber);
        }
    }

    public void ResetGame ()
    {
        currentObjectIndex = 0;
        SetCard();
    }

}
