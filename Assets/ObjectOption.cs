using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SubjectsManager;

public class ObjectOption : MonoBehaviour
{
    public Subject subject;

    [SerializeField] private Image image;

    private SubjectObjectsManager subjectObjectsManager;
    private ToriObject toriObject;


    public void SetOption ( ToriObject toriObject, SubjectObjectsManager subjectObjectsManager )
    {
        this.toriObject = toriObject;
        this.subjectObjectsManager = subjectObjectsManager;

        InitializeOption();
    }

    private void InitializeOption ()
    {
        switch (subject)
        {
            case Subject.Colors:
                image.color = toriObject.color;
                break;

            case Subject.Shapes:
                image.sprite = toriObject.sprite;
                break;

            case Subject.Animals:
                image.sprite = toriObject.sprite;
                break;
        }
    }

    public void OnOptionClicked ()
    {
        subjectObjectsManager.SelectObject(toriObject);
    }
}