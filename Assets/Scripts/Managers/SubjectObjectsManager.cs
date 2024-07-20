using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SubjectsManager;

public class SubjectObjectsManager : MonoBehaviour
{
    [SerializeField] private Subject subject;
    [SerializeField] private string nextSceneName;

    [SerializeField] private ObjectOption optionPrefab;
    [SerializeField] private Transform optionsParent;
    [SerializeField] private ScrollRect scrollRect;

    [SerializeField] private float scrollStep = 0.1f;

    [SerializeField] private SelectedObjectsUI selectedObjectsUI;

    private List<ToriObject> allSubjectObjects;
    private List<ToriObject> selectedObjects = new List<ToriObject>();
    private List<ObjectOption> objectOptions = new List<ObjectOption>();

    [SerializeField] private int maxOptions = 5;
    [SerializeField] private Button startButton;
    [SerializeField] private SceneLoader sceneLoader;

    private void Start ()
    {
        GetSubjectObjects();
        SetSubjectOptions();
    }

    private void GetSubjectObjects ()
    {
        allSubjectObjects = SubjectsManager.Instance.GetSubject(subject.name);
    }

    private void SetSubjectOptions ()
    {
        foreach (ToriObject toriObject in allSubjectObjects)
        {
            ObjectOption objectOption = Instantiate(optionPrefab, optionsParent);
            objectOption.SetOption(toriObject, this);
            objectOptions.Add(objectOption);
        }

        // Force the layout to update
        LayoutRebuilder.ForceRebuildLayoutImmediate(optionsParent.GetComponent<RectTransform>());

        // Set scroll position to the top in the next frame
        StartCoroutine(SetScrollPositionNextFrame());
    }

    private IEnumerator SetScrollPositionNextFrame ()
    {
        yield return null; // Wait for the next frame
        scrollRect.verticalNormalizedPosition = 1f;
    }

    public void ScrollUp ()
    {
        float newPosition = scrollRect.verticalNormalizedPosition + scrollStep;
        scrollRect.verticalNormalizedPosition = Mathf.Clamp(newPosition, 0f, 1f);
    }

    public void ScrollDown ()
    {
        float newPosition = scrollRect.verticalNormalizedPosition - scrollStep;
        scrollRect.verticalNormalizedPosition = Mathf.Clamp(newPosition, 0f, 1f);
    }

    public void SelectObject ( ToriObject obj )
    {
        if (selectedObjects.Count < maxOptions)
        {
            if (!selectedObjects.Contains(obj))
            {
                selectedObjects.Add(obj);
                selectedObjectsUI.AddObjectUI(obj);
                UpdateObjectOptionState(obj, false);
                CheckAndDisableAllOptions();
                UpdateStartButtonState();
            }
            else
            {
                Debug.LogWarning("Object already selected: " + obj.objectName);
            }
        }
        else
        {
            Debug.LogWarning("Maximum number of options selected.");
        }
    }

    public void RemoveObject ( ToriObject obj )
    {
        if (selectedObjects.Remove(obj))
        {
            selectedObjectsUI.RemoveObjectUI(obj);
            UpdateObjectOptionState(obj, true);
            CheckAndDisableAllOptions();
            UpdateStartButtonState();
        }
        else
        {
            Debug.LogWarning("Object not found in selected list: " + obj.objectName);
        }
    }

    private void UpdateObjectOptionState ( ToriObject obj, bool isActive )
    {
        foreach (ObjectOption objectOption in objectOptions)
        {
            if (objectOption.GetToriObject() == obj)
            {
                objectOption.SetButtonActive(isActive);
                break;
            }
        }
    }

    private void CheckAndDisableAllOptions ()
    {
        bool disableAll = selectedObjects.Count >= maxOptions;
        foreach (ObjectOption objectOption in objectOptions)
        {
            if (!selectedObjects.Contains(objectOption.GetToriObject()))
            {
                objectOption.SetButtonActive(!disableAll);
            }
        }
    }

    private void UpdateStartButtonState ()
    {
        startButton.interactable = selectedObjects.Count >= 3;
    }

    public void OnStartClick ()
    {
        GameManager.Instance.selectedObjects = selectedObjects;
        GameManager.Instance.currentSubject = subject;

        sceneLoader.LoadScene(nextSceneName);
    }

}
