using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampController : MonoBehaviour
{
    [SerializeField] private Fader closeClampFader;
    [SerializeField] private Fader openClampFader;
   // public Transform target;

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
