using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sticker : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private AudioSource audioSource;

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
}
