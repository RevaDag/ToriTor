using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubjectsManager : MonoBehaviour
{
    [Header("Subjects")]
    [SerializeField] private List<Subject> subjects;


    private Dictionary<string, Subject> subjectDictionary;

    public Subject selectedSubject { get; private set; }

    public List<ToriObject> toriObjects1;
    public List<ToriObject> toriObjects2;
    public List<ToriObject> toriObjects3;
    public List<ToriObject> toriObjects4;


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

    #region Getters

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
    public List<ToriObject> GetAllToriObjects ()
    {
        var allObjects = new List<ToriObject>();
        foreach (var subject in subjects)
        {
            allObjects.AddRange(subject.toriObjects);
        }
        return allObjects;
    }

    public List<ToriObject> GetObjectsByListNumber ( int listNumber )
    {
        switch (listNumber)
        {
            case 0:
                return null;
            case 1:
                return toriObjects1;
            case 2:
                return toriObjects2;
            case 3:
                return toriObjects3;
            case 4:
                return toriObjects4;
            default:
                return null;
        }
    }

    #endregion

    public void SelectSubjectByName ( string subjectName )
    {
        if (subjectDictionary.TryGetValue(subjectName, out Subject subject))
        {
            selectedSubject = subject;
        }
        else
        {
            Debug.LogWarning("Subject not found: " + subjectName);
            selectedSubject = null;
        }

        SplitToriObjects();
    }

    public void SplitToriObjects ()
    {
        if (selectedSubject == null || selectedSubject.toriObjects == null || selectedSubject.toriObjects.Count == 0)
        {
            Debug.LogWarning("No selected subject or tori objects to split.");
            return;
        }

        toriObjects1.Clear();
        toriObjects2.Clear();
        toriObjects3.Clear();
        toriObjects4.Clear();

        // Create a list to hold the indices
        List<int> indices = Enumerable.Range(0, selectedSubject.toriObjects.Count).ToList();

        // Shuffle the indices
        indices = indices.OrderBy(x => Random.value).ToList();

        // Distribute tori objects into the lists
        for (int i = 0; i < indices.Count; i++)
        {
            int targetList = i % 4;
            ToriObject toriObject = selectedSubject.toriObjects[indices[i]];

            switch (targetList)
            {
                case 0:
                    if (toriObjects1.Count < 3)
                        toriObjects1.Add(toriObject);
                    break;
                case 1:
                    if (toriObjects2.Count < 3)
                        toriObjects2.Add(toriObject);
                    break;
                case 2:
                    if (toriObjects3.Count < 3)
                        toriObjects3.Add(toriObject);
                    break;
                case 3:
                    if (toriObjects4.Count < 3)
                        toriObjects4.Add(toriObject);
                    break;
            }

            // Exit loop if all lists have 3 items
            if (toriObjects1.Count >= 3 && toriObjects2.Count >= 3 && toriObjects3.Count >= 3 && toriObjects4.Count >= 3)
                break;
        }
    }
}
