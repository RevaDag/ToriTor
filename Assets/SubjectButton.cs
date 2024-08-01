using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectButton : MonoBehaviour
{
    [SerializeField] private string subjectName;
    [SerializeField] private string nextSceneName;
    [SerializeField] private SceneLoader sceneLoader;
    private AudioSource audioSource;

    private void Awake ()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnClick ()
    {
        audioSource.Play();
        SubjectsManager.Instance.SelectSubjectByName(subjectName);
        sceneLoader.LoadScene(nextSceneName);
    }
}
