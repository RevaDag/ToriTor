using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Level
{
    public int LevelNumber;
    public bool IsUnlocked;
    public bool IsOpened;
    public bool IsCompleted;

    public string Subject;
    public List<ToriObject> ToriObjects;
    public int StepsNumber;


    public void UnlockLevel ()
    {
        IsUnlocked = true;
    }

    public void OpenLevel ()
    {
        IsOpened = true;
    }

    public void CompleteLevel ()
    {
        IsCompleted = true;
    }
}
