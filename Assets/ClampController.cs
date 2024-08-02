using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampController : MonoBehaviour
{
    [SerializeField] private Fader closeClampFader;
    [SerializeField] private Fader openClampFader;


    private void Start ()
    {
        MusicManager.Instance.PlayAudioClip(MusicManager.Instance.underwaterMusic);
    }
    public void OpenClamp ()
    {
        openClampFader.FadeIn();
        closeClampFader.FadeOut();
    }

    public void CloseClamp ()
    {
        closeClampFader.FadeIn();
        openClampFader.FadeOut();
    }
}
