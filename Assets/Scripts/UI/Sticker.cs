using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sticker : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private CanvasGroup canvasGroup;

    public ToriObject toriObject { get; private set; }

    private Vector3 originalScale;

    private void Awake ()
    {
        originalScale = GetComponent<RectTransform>().localScale;
    }

    public void SetToriObject ( ToriObject _toriObject )
    {
        toriObject = _toriObject;
    }

    public void SetImage ( Sprite sprite )
    {
        image.sprite = sprite;
        image.SetNativeSize();
    }

    public void ResizeImageScale ( Vector3 _scale )
    {
        image.GetComponent<RectTransform>().localScale = _scale;
    }

    public void SetAudio ( AudioClip clip )
    {
        audioSource.clip = clip;
    }

    public void SetColor ( Color color )
    {
        image.color = color;
    }

    public Color GetColor ()
    {
        return image.color;
    }


    public void ActivateGroupCanvas ( bool isActive )
    {
        canvasGroup.interactable = isActive;
        canvasGroup.alpha = isActive ? 1f : 0.5f;
    }

    public void PlayAudio ( float _volume = 1f )
    {
        audioSource.volume = _volume;
        audioSource.Play();
    }

    public void ResetScale ()
    {
        ResizeImageScale(originalScale);
    }

}
