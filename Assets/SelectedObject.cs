using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SubjectsManager;

public class SelectedObject : MonoBehaviour
{
    public Subject subject;
    [SerializeField] private Image image;
    [SerializeField] private AudioSource audioSource;

    public void SetObjectUI ( ToriObject toriObject )
    {
        audioSource.clip = toriObject.clip;

        switch (subject)
        {
            case Subject.Colors:
                image.color = toriObject.color;
                break;
        }
    }

    public void OnSelectedObjectClicked ()
    {
        audioSource.Play();

    }
}
