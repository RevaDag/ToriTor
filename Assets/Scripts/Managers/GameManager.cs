using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<ToriObject> selectedObjects;
    public Subject currentSubject;
    public int currentLevel;

    [Header("Player Progress")]
    private string saveFilePath;
    public List<LearnedSubject> learnedSubjects { get; private set; }

    private Dictionary<string, Subject> subjectDictionary;

    public enum GameType
    {
        Book,
        Chest,
        Speech,
        Matching
    }

    void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Set the file path for saving progress
            saveFilePath = Path.Combine(Application.persistentDataPath, "playerProgress.json");

            InitializeSubjectDictionary();
            learnedSubjects = new List<LearnedSubject>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start ()
    {
        LoadProgress();
    }

    private void InitializeSubjectDictionary ()
    {
        subjectDictionary = SubjectsManager.Instance?.GetAllSubjects().ToDictionary(sub => sub.name, sub => sub);
    }

    public void ProgressToNextLevel ()
    {
        currentLevel++;
    }

    #region Progress
    public void SaveProgress ()
    {
        // Find the learned subject
        var learnedSubject = learnedSubjects.FirstOrDefault(ls => ls.GetSubject().name == currentSubject.name);
        if (learnedSubject == null)
        {
            learnedSubject = new LearnedSubject(currentSubject);
            learnedSubjects.Add(learnedSubject);
        }

        // Add selected objects to learned objects within the current subject
        foreach (var obj in selectedObjects)
        {
            if (!learnedSubject.learnedObjects.Contains(obj))
            {
                learnedSubject.learnedObjects.Add(obj);
            }
        }

        // Create progress data object
        ProgressData progressData = new ProgressData
        {
            learnedSubjects = learnedSubjects.Select(ls => new LearnedSubjectData
            {
                subjectName = ls.GetSubject().name,
                learnedObjectNames = ls.learnedObjects.Select(obj => obj.objectName).ToList()
            }).ToList()
        };

        // Serialize the progress data to JSON
        string json = JsonUtility.ToJson(progressData, true);

        // Save the JSON to a file
        File.WriteAllText(saveFilePath, json);
    }

    public void LoadProgress ()
    {
        if (File.Exists(saveFilePath))
        {
            // Read the JSON from the file
            string json = File.ReadAllText(saveFilePath);

            // Deserialize the JSON to a progress data object
            ProgressData progressData = JsonUtility.FromJson<ProgressData>(json);

            // Load the learned subjects and their objects
            learnedSubjects = new List<LearnedSubject>();

            foreach (var learnedSubjectData in progressData.learnedSubjects)
            {
                if (subjectDictionary.TryGetValue(learnedSubjectData.subjectName, out var subject))
                {
                    var learnedSubject = new LearnedSubject(subject);
                    learnedSubject.learnedObjects = learnedSubjectData.learnedObjectNames
                        .Select(name => FindToriObjectByName(name))
                        .Where(obj => obj != null)
                        .ToList();
                    learnedSubjects.Add(learnedSubject);
                }
            }
        }
    }

    public void ResetLevel ()
    {
        currentLevel = 0;
    }

    [ContextMenu("Reset Progress")]
    public void ResetProgress ()
    {
        currentLevel = 0;
        learnedSubjects.Clear();
        SaveProgress();
        LoadProgress();
    }
    #endregion

    private ToriObject FindToriObjectByName ( string name )
    {
        // Search through all ToriObjects managed by SubjectsManager
        var allToriObjects = SubjectsManager.Instance.GetAllToriObjects();
        return allToriObjects.FirstOrDefault(obj => obj.objectName == name);
    }

    public List<ToriObject> GetLearnedObjectsBySubject ( LearnedSubject _learnedSubject )
    {
        var learnedSubject = learnedSubjects.FirstOrDefault(ls => ls.subject.name == _learnedSubject.subject.name);
        return learnedSubject?.learnedObjects ?? new List<ToriObject>();
    }
}


[System.Serializable]
public class ProgressData
{
    public List<LearnedSubjectData> learnedSubjects;
}

[System.Serializable]
public class LearnedSubjectData
{
    public string subjectName;
    public List<string> learnedObjectNames;
}

