using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectButton : MonoBehaviour
{
    [SerializeField] private string subjectName;
    [SerializeField] private string nextSceneName;
    [SerializeField] private SceneLoader sceneLoader;

    public void OnClick ()
    {
        SubjectsManager.Instance.SelectSubjectByName(subjectName);
        sceneLoader.LoadScene(nextSceneName);
    }
}
