using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Question : MonoBehaviour
{
    public RectTransform target;

    [SerializeField] private List<Image> imageSprites;
    [SerializeField] private Image imageToColor;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private Fader questionFader;


    [Header("Additionals")]
    public FindCard findCard;
    public Sticker sticker;
    public Animator animator;


    public ToriObject toriObject { get; private set; }

    public void SetObject ( ToriObject obj )
    {
        toriObject = obj;
    }


    public void SetAudioClip ( AudioClip _clip )
    {
        audioSource.clip = _clip;
    }

    public void PlayAudioSouce ()
    {
        audioSource.Play();
    }

    public void SetImages ( Sprite _sprite )
    {
        foreach (Image image in imageSprites)
        {
            image.sprite = _sprite;
            image.SetNativeSize();
        }
    }


    public void ColorImage ( Color _color )
    {
        imageToColor.color = _color;
    }

    public void FadeIn ()
    {
        questionFader.FadeIn();
    }

    public void FadeOut ()
    {
        questionFader.FadeOut();
    }

}
