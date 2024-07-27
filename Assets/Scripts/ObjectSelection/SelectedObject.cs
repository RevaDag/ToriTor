using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedObject : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private AudioSource audioSource;


    private Subject subject;
    private ToriObject toriObject;
    private SelectedObjectsUI selectedObjectsUI;

    public void SetSubject ( Subject _subject )
    {
        this.subject = _subject;
    }

    public void SetObjectUI ( ToriObject _toriObject, SelectedObjectsUI _selectedObjectsUI )
    {
        selectedObjectsUI = _selectedObjectsUI;
        toriObject = _toriObject;


        switch (subject.name)
        {
            case "Colors":
                image.color = _toriObject.color;
                break;
            case "Shapes":
                image.sprite = _toriObject.sprite;
                image.SetNativeSize();
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
