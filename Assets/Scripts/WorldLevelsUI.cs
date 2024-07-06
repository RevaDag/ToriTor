using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldLevelsUI : MonoBehaviour
{
    [SerializeField] private List<LevelButton> levelButtons;

    private void Start ()
    {
        //UpdateWorldsUI();
    }

    /*   public void UpdateWorldsUI ()
       {
           List<Level> levels = GameManager.Instance.playerProgress.Levels;

           foreach (Level level in levels)
           {
               levelButtons[level.LevelNumber - 1].SetLevelNumber(level.LevelNumber);

               if (level.IsCompleted)
               {
                   OpenLevel(level.LevelNumber);
                   Debug.Log($"Level {level.LevelNumber} is Complete.");
               }
               else if (level.IsOpened)
               {
                   OpenLevel(level.LevelNumber);
                   Debug.Log($"Level {level.LevelNumber} is Open.");
               }
               else if (level.IsUnlocked)
               {
                   UnlockLevel(level.LevelNumber);
                   Debug.Log($"Level {level.LevelNumber} is Unlocked.");
               }
               else
               {
                   LockLevel(level.LevelNumber);
                   Debug.Log($"Level {level.LevelNumber} is Locked.");
               }
           }
       }
   */


    public void LockLevel ( int levelNumber )
    {
        if (levelNumber > 0 && levelNumber <= levelButtons.Count)
        {
            levelButtons[levelNumber - 1].SetLevelButtonState(0); // Locked
        }
    }

    public void UnlockLevel ( int levelNumber )
    {
        if (levelNumber > 0 && levelNumber <= levelButtons.Count)
        {
            levelButtons[levelNumber - 1].SetLevelButtonState(1); // Unlocked
            //GameManager.Instance.UnlockLevelByLevelNumber(levelNumber);
        }
    }

    public void OpenLevel ( int levelNumber )
    {
        if (levelNumber > 0 && levelNumber <= levelButtons.Count)
        {
            levelButtons[levelNumber - 1].SetLevelButtonState(2); // Open
            //GameManager.Instance.OpenLevelByLevelNumber(levelNumber);
        }
    }
}
