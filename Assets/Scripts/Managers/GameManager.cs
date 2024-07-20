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
    public List<Subject> learnedSubjects { get; private set; }
    public List<ToriObject> learnedObjects { get; private set; }

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
            learnedSubjects = new List<Subject>();
            learnedObjects = new List<ToriObject>();
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
        subjectDictionary = SubjectsManager.Instance.GetAllSubjects().ToDictionary(sub => sub.name, sub => sub);
    }

    public void ProgressToNextLevel ()
    {
        currentLevel++;
    }

    public void ResetProgress ()
    {
        currentLevel = 0;
    }

    #region Save & Load
    public void SaveProgress ()
    {
        // Add selected objects to learned objects if not already present
        foreach (var obj in selectedObjects)
        {
            if (!learnedObjects.Contains(obj))
            {
                learnedObjects.Add(obj);
            }
        }

        // Add current subject to learned subjects if not already present
        if (!learnedSubjects.Contains(currentSubject))
        {
            learnedSubjects.Add(currentSubject);
        }

        // Get the names of the learned objects
        List<string> learnedObjectNames = learnedObjects.Select(obj => obj.objectName).ToList();

        // Get the names of the learned subjects
        List<string> learnedSubjectNames = learnedSubjects.Select(sub => sub.name).ToList();

        // Create a progress data object
        ProgressData progressData = new ProgressData
        {
            learnedObjects = learnedObjectNames,
            learnedSubjects = learnedSubjectNames
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

            // Load the learned objects
            learnedObjects = progressData.learnedObjects.Select(name => FindToriObjectByName(name)).ToList();

            // Load the learned subjects
            learnedSubjects = progressData.learnedSubjects.Select(name => FindSubjectByName(name)).ToList();
        }
    }
    #endregion

    private ToriObject FindToriObjectByName ( string name )
    {
        // Implement the logic to find and return the ToriObject by name
        return selectedObjects.Concat(learnedObjects).FirstOrDefault(obj => obj.objectName == name);
    }

    private Subject FindSubjectByName ( string name )
    {
        if (subjectDictionary.TryGetValue(name, out var subject))
        {
            return subject;
        }

        else return null;

    }


}

[System.Serializable]
public class ProgressData
{
    public List<string> learnedObjects;
    public List<string> learnedSubjects;
}

