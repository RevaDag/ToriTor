using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSummary : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Fader fader;
    [SerializeField] private ParticleSystem particaleSystem;
    [SerializeField] private Fader textButtonFader;

    public void PlayMapSummary ()
    {
        StartCoroutine(MapSummaryCoroutine());
    }

    private IEnumerator MapSummaryCoroutine ()
    {
        fader.FadeIn();
        yield return new WaitForSeconds(1);

        animator.SetTrigger("Open");
        yield return new WaitForSeconds(1);

        particaleSystem.Play();
        textButtonFader.FadeIn();
    }


}
