using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectsManager : MonoBehaviour
{
    public enum Subject
    {
        Colors,
        Shapes,
        Animals
    }

    [SerializeField] private List<ToriObject> Colors;
    [SerializeField] private List<ToriObject> Animals;
    [SerializeField] private List<ToriObject> Shapes;

    private Dictionary<Subject, List<ToriObject>> subjectsDictionary;

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

        subjectsDictionary = new Dictionary<Subject, List<ToriObject>>
        {
            { Subject.Colors, Colors },
            { Subject.Animals, Animals },
            { Subject.Shapes, Shapes }
        };
    }

    public List<ToriObject> GetSubject ( Subject subject )
    {
        if (subjectsDictionary.TryGetValue(subject, out List<ToriObject> subjectList))
        {
            return subjectList;
        }
        else
        {
            Debug.LogWarning("Subject not found: " + subject);
            return null;
        }
    }
}
