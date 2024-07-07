using System.Collections.Generic;
using UnityEngine;
using static SubjectsManager;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<ToriObject> selectedObjects;

    public Subject currentSubject;

    public enum GameType
    {
        Book,
        Chest,
        Speech,
        Matching
    }

    public ToriObject GetToriObject ( int index )
    {
        if (index <= selectedObjects.Count)
            return selectedObjects[index];
        else
            return null;
    }



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



}
