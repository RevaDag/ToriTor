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

}
