using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitModal : MonoBehaviour
{
    [SerializeField] private AudioSource buttonsSFX;
    private Fader fader;

    private void Awake ()
    {
        fader = GetComponent<Fader>();
    }

    public void FadeIn ()
    {
        fader.FadeIn();
    }

    public void FadeOut ()
    {
        fader.FadeOut();
    }

    public void OnYesClicked ()
    {
        buttonsSFX.Play();
        LoadingScreen.Instance.LoadPreviousScene();
    }

    public void OnNoClicked ()
    {
        buttonsSFX.Play();
        FadeOut();
    }
}
