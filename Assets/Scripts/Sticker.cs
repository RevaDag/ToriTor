using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sticker : MonoBehaviour
{

    [SerializeField] private Image image;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TMP_Text text;
    [SerializeField] private CanvasGroup canvasGroup;

    private ToriObject toriObject;

    public void SetToriObject ( ToriObject _toriObject )
    {
        toriObject = _toriObject;
    }

    public void SetImage ( Sprite sprite )
    {
        image.sprite = sprite;
        image.SetNativeSize();
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

    public void SetText ( string _text )
    {
        text.text = _text;
    }

    public void ActivateGroupCanvas ( bool isActive )
    {
        canvasGroup.interactable = isActive;
        canvasGroup.alpha = isActive ? 1f : 0.5f;
    }
}
