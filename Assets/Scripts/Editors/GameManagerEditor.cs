using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI ()
    {
        DrawDefaultInspector();

        GameManager gameManager = (GameManager)target;
        if (GUILayout.Button("Reset Progress"))
        {
            gameManager.ResetProgress();
        }
    }
}
