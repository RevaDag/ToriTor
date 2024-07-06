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


    private ToriObject toriObject;
    private SelectedObjectsUI selectedObjectsUI;

    public void SetObjectUI ( ToriObject _toriObject, SelectedObjectsUI _selectedObjectsUI )
    {
        selectedObjectsUI = _selectedObjectsUI;
        toriObject = _toriObject;


        switch (subject)
        {
            case Subject.Colors:
                image.color = _toriObject.color;
                break;
        }

        audioSource.clip = _toriObject.clip;
    }

    public ToriObject GetToriObject ()
    {
        return toriObject;
    }

    public void OnSelectedObjectClicked ()
    {
        audioSource.Play();

        selectedObjectsUI.RemoveObjectUI(toriObject);
    }
}
