using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Question : MonoBehaviour
{
    public RectTransform target;

    private Image image;
    private AudioSource audioSource;


    private void Awake ()
    {
        audioSource = GetComponent<AudioSource>();
        image = GetComponent<Image>();
    }

    public void SetQuestion ( ToriObject toriObject )
    {
        image.sprite = toriObject.parallelObjectSprite;
        image.SetNativeSize();

        audioSource.clip = toriObject.parallelObjectClip;
    }
}
