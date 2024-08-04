using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Question : MonoBehaviour
{
    public RectTransform target;

    [SerializeField] private Image imageSprite;
    [SerializeField] private Image imageToColor;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private Animator animator;

    [Header("Faders")]
    [SerializeField] private Fader questionFader;
    [SerializeField] private Fader checkFader;

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

    public void SetImage ( Sprite _sprite )
    {
        imageSprite.sprite = _sprite;
        imageSprite.SetNativeSize();
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

    public void FadeInCheckMark ()
    {
        checkFader.FadeIn();
    }

    public void FlipCard ()
    {
        animator.SetTrigger("Flip");
    }
}
