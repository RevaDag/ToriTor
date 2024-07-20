using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SubjectsManager;

public class ObjectOption : MonoBehaviour
{
    public Subject subject;

    [SerializeField] private Image image;
    [SerializeField] private Button button;

    private SubjectObjectsManager subjectObjectsManager;
    private ToriObject toriObject;


    public void SetOption ( ToriObject toriObject, SubjectObjectsManager subjectObjectsManager )
    {
        this.toriObject = toriObject;
        this.subjectObjectsManager = subjectObjectsManager;

        InitializeOption();
    }

    public ToriObject GetToriObject ()
    {
        return toriObject;
    }

    private void InitializeOption ()
    {
        switch (subject.name)
        {
            case "Colors":
                image.color = toriObject.color;
                break;

            case "Shapes":
                image.sprite = toriObject.sprite;
                break;

            case "Animals":
                image.sprite = toriObject.sprite;
                break;
        }

        image.SetNativeSize();
    }

    public void OnOptionClicked ()
    {
        subjectObjectsManager.SelectObject(toriObject);
        SetButtonActive(false);
    }

    public void SetButtonActive ( bool isActive )
    {
        button.interactable = isActive;
    }
}
