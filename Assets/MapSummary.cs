using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSummary : MonoBehaviour
{
    [SerializeField] private CustomSpriteAnimator animator;
    [SerializeField] private Fader fader;

    public void PlayMapSummary ()
    {
        StartCoroutine(MapSummaryCoroutine());
    }

    private IEnumerator MapSummaryCoroutine ()
    {
        fader.FadeIn();
        yield return new WaitForSeconds(1);

        PlayAnimation();
    }


    public void PlayAnimation ()
    {
        animator.StartAnimation();
    }
}
