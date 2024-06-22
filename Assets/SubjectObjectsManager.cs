using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SubjectsManager;

public class SubjectObjectsManager : MonoBehaviour
{
    [SerializeField] private Subject subject;

    [SerializeField] private ObjectOption optionPrefab;
    [SerializeField] private Transform optionsParent;

    [SerializeField] private SelectedObjectsUI selectedObjectsUI;

    private List<ToriObject> allSubjectObjects;
    private List<ToriObject> selectedObjects = new List<ToriObject>();

    [SerializeField] private int maxOptions;

    private void Start ()
    {
        GetSubjectObjects();
        SetSubjectOptions();
    }

    private void GetSubjectObjects ()
    {
        allSubjectObjects = SubjectsManager.Instance.GetSubject(subject);
    }

    private void SetSubjectOptions ()
    {
        foreach (ToriObject toriObject in allSubjectObjects)
        {
            ObjectOption objectOption = Instantiate(optionPrefab, optionsParent);
            objectOption.SetOption(toriObject, this);
        }
    }

    public void SelectObject ( ToriObject obj )
    {
        if (!selectedObjects.Contains(obj))
        {
            selectedObjects.Add(obj);
            selectedObjectsUI.AddObjectUI(obj);
        }
        else
        {
            Debug.LogWarning("Object already selected: " + obj.objectName);
        }
    }

    public void RemoveObject ( ToriObject obj )
    {
        selectedObjects.Remove(obj);
    }

    public void OnStartClick ()
    {

    }
}
