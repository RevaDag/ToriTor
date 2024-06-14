using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToriTheCat : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Fader toriFader;

    public void FadeIn ()
    {
        toriFader.FadeIn();
    }

    public void FadeOut ()
    {
        toriFader.FadeOut();
    }

    public void SlideUp ()
    {
        animator.SetTrigger("SlideUp");
    }

    public void SlideDown ()
    {
        animator.SetTrigger("SlideDown");
    }
}
