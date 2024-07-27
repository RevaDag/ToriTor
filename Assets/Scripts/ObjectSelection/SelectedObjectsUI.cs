using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SubjectsManager;


public class SelectedObjectsUI : MonoBehaviour
{

    [SerializeField] private SubjectObjectsManager subjectObjectsManager;

    [SerializeField] private List<Transform> objectTransforms;

    [SerializeField] private SelectedObject selectedObjectPrefab;

    private List<ToriObject> selectedObjects = new List<ToriObject>();
    private Subject subject;

    public void SetSubject ( Subject _subject )
    {
        subject = _subject;
    }


    public void AddObjectUI ( ToriObject toriObject )
    {
        // Find the last available position in the transform list
        Transform availablePosition = null;
        foreach (Transform position in objectTransforms)
        {
            if (position.childCount == 0)
            {
                availablePosition = position;
                break;
            }
        }

        // If no available position is found, log a warning and return
        if (availablePosition == null)
        {
            Debug.LogWarning("No available position to add the object.");
            return;
        }

        // Instantiate the prefab at the available position
        SelectedObject selectedObject = Instantiate(selectedObjectPrefab, availablePosition);

        // Add the ToriObject to the selectedObjects list
        selectedObjects.Add(toriObject);

        selectedObject.SetSubject(subject);
        selectedObject.SetObjectUI(toriObject, this);
    }

    public void RemoveObjectUI ( ToriObject toriObject )
    {
        // Find the UI element associated with the ToriObject
        SelectedObject selectedObjectToRemove = null;
        foreach (Transform position in objectTransforms)
        {
            if (position.childCount > 0)
            {
                SelectedObject selectedObject = position.GetChild(0).GetComponent<SelectedObject>();
                if (selectedObject != null && selectedObject.GetToriObject() == toriObject)
                {
                    selectedObjectToRemove = selectedObject;
                    break;
                }
            }
        }

        // If the UI element is found, remove the ToriObject and destroy the UI element
        if (selectedObjectToRemove != null)
        {
            subjectObjectsManager.RemoveObject(toriObject);
            selectedObjects.Remove(toriObject);
            Destroy(selectedObjectToRemove.gameObject);
        }
        else
        {
            Debug.LogWarning("Object not found in UI: " + toriObject.objectName);
        }
    }
}

