using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectButton : MonoBehaviour
{
    [SerializeField] private string subjectName;
    [SerializeField] private string nextSceneName;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private AudioSource buttonSFX;

    public void OnClick ()
    {
        buttonSFX.Play();
        SubjectsManager.Instance.SelectSubjectByName(subjectName);
        sceneLoader.LoadScene(nextSceneName);
    }
}
