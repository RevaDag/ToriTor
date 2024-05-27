using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Level
{
    public int LevelNumber;
    public LevelButton LevelButton;
    public bool IsOpened;
    public bool IsCompleted;

    public Level ( int levelNumber, LevelButton levelButton, bool isOpened, bool isCompleted )
    {
        LevelNumber = levelNumber;
        LevelButton = levelButton;
        IsOpened = isOpened;
        IsCompleted = isCompleted;
    }
}
