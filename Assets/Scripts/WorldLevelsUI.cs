using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldLevelsUI : MonoBehaviour
{
    [SerializeField] private List<LevelButton> levelButtons;

    private void Start ()
    {
        UpdateWorldsUI();
    }

    public void UpdateWorldsUI ()
    {
        List<Level> levels = GameManager.Instance.playerProgress.Levels;

        foreach (Level level in levels)
        {
            if (level.IsCompleted)
            {
                OpenLevel(level);
                Debug.Log($"Level {level.LevelNumber} is Complete.");
            }
            else if (level.IsOpened)
            {
                OpenLevel(level);
                Debug.Log($"Level {level.LevelNumber} is Open.");
            }
            else if (level.IsUnlocked)
            {
                UnlockLevel(level);
                Debug.Log($"Level {level.LevelNumber} is Unlocked.");
            }
            else
            {
                LockLevel(level);
                Debug.Log($"Level {level.LevelNumber} is Locked.");
            }
        }
    }



    public void LockLevel ( Level level )
    {
        if (level.LevelNumber > 0 && level.LevelNumber <= levelButtons.Count)
        {
            levelButtons[level.LevelNumber - 1].SetLevelButtonState(0); // Locked
        }
    }

    public void UnlockLevel ( Level level )
    {
        if (level.LevelNumber > 0 && level.LevelNumber <= levelButtons.Count)
        {
            levelButtons[level.LevelNumber - 1].SetLevelButtonState(1); // Unlocked
            GameManager.Instance.UnlockLevel(level);
        }
    }

    public void OpenLevel ( Level level )
    {
        if (level.LevelNumber > 0 && level.LevelNumber <= levelButtons.Count)
        {
            levelButtons[level.LevelNumber - 1].SetLevelButtonState(2); // Open
            GameManager.Instance.OpenLevel(level);
        }
    }
}
