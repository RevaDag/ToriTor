using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SubjectsManager;


public class SelectedObjectsUI : MonoBehaviour
{
    public Subject subject;

    [SerializeField] private List<Transform> objectTransforms;

    [SerializeField] private SelectedObject selectedObjectPrefab;

    private List<ToriObject> selectedObjects = new List<ToriObject>();

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

        selectedObject.SetObjectUI(toriObject);
    }
}
