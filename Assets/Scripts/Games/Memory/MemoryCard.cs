using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCard : MonoBehaviour
{
    [SerializeField] private MemoryGame memoryGame;
    public Sticker sticker;
    [SerializeField] private Fader imageFader;
    private Animator animator;
    private ToriObject toriObject;
    private bool isClicked = true;
    private bool isMatched = false;


    private void Awake ()
    {
        animator = GetComponent<Animator>();
    }

    public void DeployCard ( ToriObject _cardObject )
    {
        toriObject = _cardObject;

        sticker.SetToriObject(_cardObject);
        sticker.SetImage(_cardObject.sprite);
        sticker.SetAudio(_cardObject.clip);
    }

    public void OnCardClicked ()
    {
        if (isClicked) return;

        if (isMatched)
        {
            sticker.PlayAudio();
            return;
        }
        else if (memoryGame.CanRevealCard())
        {
            RevealObject();
            memoryGame.CardRevealed(this);
            sticker.PlayAudio();
        }

    }

    public void RevealObject ()
    {
        animator.SetTrigger("Flip");
        imageFader.FadeIn();

        isClicked = true;
    }

    public void HideObject ()
    {
        animator.SetTrigger("Flip");
        imageFader.FadeOut();

        isClicked = false;
    }

    public void ShowParallel ()
    {
        isMatched = true;
        StartCoroutine(ShowParallelCoroutine());
    }

    private IEnumerator ShowParallelCoroutine ()
    {
        HideObject();
        yield return new WaitForSeconds(1);
        sticker.SetImage(toriObject.parallelObjectSprite);
        sticker.SetAudio(toriObject.parallelObjectClip);
        RevealObject();
        sticker.PlayAudio(.5f);
    }

    public void ResetCard ()
    {
        isClicked = false;
        isMatched = false;
    }


}
