using System.Collections.Generic;
using UnityEngine;

public class SubjectsManager : MonoBehaviour
{
    [Header("Subjects")]
    [SerializeField] private List<Subject> subjects;


    private Dictionary<string, Subject> subjectDictionary;

    public static SubjectsManager Instance { get; private set; }

    private void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitiateSubjectDictionary();
    }

    private void InitiateSubjectDictionary ()
    {
        subjectDictionary = new Dictionary<string, Subject>();

        foreach (var subject in subjects)
        {
            subjectDictionary[subject.name] = subject;
        }
    }

    public List<ToriObject> GetSubject ( string subjectName )
    {
        if (subjectDictionary.TryGetValue(subjectName, out Subject subject))
        {
            return subject.toriObjects;
        }
        else
        {
            Debug.LogWarning("Subject not found: " + subjectName);
            return null;
        }
    }

    public List<Subject> GetAllSubjects ()
    {
        return subjects;
    }
}
