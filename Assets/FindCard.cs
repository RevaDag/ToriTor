using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindCard : MonoBehaviour
{
    [SerializeField] private Fader checkFader;
    [SerializeField] private Animator animator;


    public void FadeInCheckMark ()
    {
        checkFader.FadeIn();
    }

    public void FadeOutCheckMark ()
    {
        checkFader.FadeOut();
    }

    public void FlipCard ()
    {
        animator.SetTrigger("Flip");
    }

    public void ResetCard ()
    {
        FlipCard();
        FadeOutCheckMark();
    }
}
