using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CardsController : MonoBehaviour
{
    [SerializeField] private Sticker sticker;
    [SerializeField] private Animator cardAnimator;
    [SerializeField] private QuizSummary quizSummary;

    private bool isParallel;

    private ToriObject currentObject;
    private int currentObjectIndex;
    private int maxObjects = 3;

    private void Start ()
    {
        _ = LoadingScreen.Instance.HideLoadingScreen();
        SetCard();
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
        if (currentObjectIndex < maxObjects - 1)
        {
            currentObjectIndex++;
            SetCard();
        }
        else
        {
            quizSummary.ShowSummary();
            GameManager.Instance.ProgressToNextLevel();
        }
    }

}
