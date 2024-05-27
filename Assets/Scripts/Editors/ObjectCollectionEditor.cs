using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectCollection))]
public class ObjectCollectionEditor : Editor
{
    public override void OnInspectorGUI ()
    {
        DrawDefaultInspector();

        ObjectCollection objectCollection = (ObjectCollection)target;
        if (GUILayout.Button("Reset Collection"))
        {
            objectCollection.ResetCollection();
            Debug.Log("Collection has been reset.");
        }
    }
}
