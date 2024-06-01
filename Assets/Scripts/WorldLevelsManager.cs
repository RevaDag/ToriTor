using System.Collections.Generic;
using UnityEngine;

public class WorldLevelsManager : MonoBehaviour
{
    public List<Level> Levels = new List<Level>();

    private void Start ()
    {
        UpdateWorldsUI();
    }

    public void UpdateWorldsUI ()
    {
        PlayerProgress playerProgress = GameManager.Instance.playerProgress;

        foreach (Level level in Levels)
        {
            if (playerProgress.LevelsOpened.Contains(level.LevelNumber))
            {
                OpenLevel(level.LevelNumber);
                Debug.Log($"Level {level.LevelNumber} is Open.");
            }
            else if (playerProgress.LevelsUnlocked.Contains(level.LevelNumber))
            {
                UnlockLevel(level.LevelNumber);
                Debug.Log($"Level {level.LevelNumber} is Unlocked.");
            }
            else
            {
                // Else, lock the level
                LockLevel(level.LevelNumber);
                Debug.Log($"Level {level.LevelNumber} is Locked.");
            }
        }
    }

    public void LockLevel ( int levelNumber )
    {
        if (levelNumber > 0 && levelNumber <= Levels.Count)
        {
            Levels[levelNumber - 1].LevelButton.SetLevelButtonState(0); // Locked
        }
    }

    public void UnlockLevel ( int levelNumber )
    {
        if (levelNumber > 0 && levelNumber <= Levels.Count)
        {
            Levels[levelNumber - 1].LevelButton.SetLevelButtonState(1); // Unlocked
        }
    }

    public void OpenLevel ( int levelNumber )
    {
        if (levelNumber > 0 && levelNumber <= Levels.Count)
        {
            Levels[levelNumber - 1].LevelButton.SetLevelButtonState(2); // Open
            GameManager.Instance.SaveOpenLevel(levelNumber);
        }
    }
}
