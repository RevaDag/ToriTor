using System.Collections.Generic;
using UnityEngine;


public class ObjectCollection : MonoBehaviour
{
    public static ObjectCollection Instance { get; private set; }

    private BookManager bookManager;
    public Dictionary<string, List<CollectibleObject>> collection;
    private List<string> subjects;

    private string subjectToLoad;
    private string subjectTitle;

    public List<CollectibleObject> tempObjects = new List<CollectibleObject>();


    void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start ()
    {
        collection = new Dictionary<string, List<CollectibleObject>>();
        subjects = new List<string>();

        LoadCollection();
    }

    public void AddObject ( CollectibleObject obj )
    {
        if (!collection.ContainsKey(obj.subject))
        {
            collection[obj.subject] = new List<CollectibleObject>();
            subjects.Add(obj.subject);
        }

        // Ensure the object isn't already in the collection by checking InstanceID
        if (!collection[obj.subject].Exists(o => o.objectPrefab.GetInstanceID() == obj.objectPrefab.GetInstanceID()))
        {
            collection[obj.subject].Add(obj);
        }
        else
        {
            Debug.Log($"Object '{obj.objectPrefab.name}' is already in the collection under subject '{obj.subject}'.");
        }

        //DEBUG
        DebugLogCollection();
        SaveCollection();
    }

    public List<CollectibleObject> GetSubjectCollection ( string subject )
    {
        if (collection.ContainsKey(subject))
        {
            return collection[subject];
        }
        else
        {
            Debug.LogWarning($"Subject '{subject}' not found in collection.");
            return null;
        }
    }

    public void ResetCollection ()
    {
        if (collection != null)
            collection.Clear();

        if (subjects != null)
            subjects.Clear();

        SaveCollection();
    }

    public void SaveCollection ()
    {
        foreach (var subject in subjects)
        {
            string json = JsonUtility.ToJson(new SerializableList<CollectibleObject>(collection[subject]));
            PlayerPrefs.SetString(subject, json);
        }

        // Save the list of subjects separately
        string subjectsJson = JsonUtility.ToJson(new SerializableList<string>(subjects));
        PlayerPrefs.SetString("subjects", subjectsJson);
        PlayerPrefs.Save();
    }

    public void LoadCollection ()
    {
        collection.Clear();
        subjects.Clear();

        // Load the list of subjects
        if (PlayerPrefs.HasKey("subjects"))
        {
            string subjectsJson = PlayerPrefs.GetString("subjects");
            SerializableList<string> loadedSubjects = JsonUtility.FromJson<SerializableList<string>>(subjectsJson);
            subjects = loadedSubjects.items;

            // Load each subject's objects
            foreach (var subject in subjects)
            {
                if (PlayerPrefs.HasKey(subject))
                {
                    string json = PlayerPrefs.GetString(subject);
                    SerializableList<CollectibleObject> objects = JsonUtility.FromJson<SerializableList<CollectibleObject>>(json);
                    collection[subject] = objects.items;
                }
            }
        }
    }

    public void DebugLogCollection ()
    {
        foreach (var kvp in collection)
        {
            string subject = kvp.Key;
            List<CollectibleObject> objects = kvp.Value;

            Debug.Log($"Subject: {subject}");
            foreach (var obj in objects)
            {
                Debug.Log($"  Object Name: {obj.objectPrefab.name}");
            }
        }
    }

    public void SetBookManager ( BookManager _bookManager )
    {
        this.bookManager = _bookManager;
    }

    public void SetSubjectToLoad ( string _subject, string _subjectTitle )
    {
        subjectToLoad = _subject;
        subjectTitle = _subjectTitle;
    }

    public void LoadObjectListAndSubject ( List<CollectibleObject> collectibleObjectsToLoad )
    {
        if (bookManager == null) return;

        bookManager.SetObjects(collectibleObjectsToLoad);
        bookManager.SetSubjectTitle(subjectTitle);
    }

    public void LoadSubjectFromCollection ()
    {
        if (bookManager == null) return;

        List<CollectibleObject> collectibleObjectsToLoad = GetSubjectCollection(subjectToLoad);
        LoadObjectListAndSubject(collectibleObjectsToLoad);
    }

    public void SetTempObjects ( List<CollectibleObject> collectibleObjects )
    {
        tempObjects?.Clear();
        tempObjects = collectibleObjects;
    }


    [System.Serializable]
    private class SerializableList<T>
    {
        public List<T> items;

        public SerializableList ( List<T> items )
        {
            this.items = items;
        }
    }
}
