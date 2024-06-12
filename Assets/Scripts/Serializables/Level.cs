using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Level
{
    public enum LevelType
    {
        Chest,
        Book,
        Speech
    }

    public LevelType Type;
    public int LevelNumber;
    public bool IsUnlocked;
    public bool IsOpened;
    public bool IsCompleted;

    public string Subject;
    public List<ToriObject> ToriObjects;
    public int StepsNumber;

    public Level ( LevelType levelType, int levelNumber, bool isUnlocked, bool isOpened, bool isCompleted, string subject, List<ToriObject> toriObjects, int stepsNumber )
    {
        Type = levelType;
        LevelNumber = levelNumber;
        IsUnlocked = isUnlocked;
        IsOpened = isOpened;
        IsCompleted = isCompleted;
        Subject = subject;
        ToriObjects = toriObjects;
        StepsNumber = stepsNumber;
    }

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
