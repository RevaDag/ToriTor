using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Level
{
    public enum LevelType
    {
        Chest,
        Book
    }

    public LevelType Type;
    public int LevelNumber;
    public bool IsUnlocked;
    public bool IsOpened;
    public bool IsCompleted;

    public Level ( LevelType levelType, int levelNumber, LevelButton levelButton, bool isUnlocked, bool isOpened, bool isCompleted )
    {
        Type = levelType;
        LevelNumber = levelNumber;
        IsUnlocked = isUnlocked;
        IsOpened = isOpened;
        IsCompleted = isCompleted;
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
