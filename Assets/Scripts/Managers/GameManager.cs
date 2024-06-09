using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerProgress playerProgress { get; private set; }
    public Level currentLevel { get; private set; }

    public List<Level> defaultLevels;

    public List<ToriObject> currentLevelObjects;



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

        playerProgress = new PlayerProgress();
        playerProgress.LoadProgress();
    }

    // Save a single completed level
    public void SaveCompletedLevel ( Level level )
    {
        playerProgress.Levels.Find(l => l.LevelNumber == level.LevelNumber).CompleteLevel();  // Mark the current level as completed

        // Unlock the next level
        int nextLevelNumber = level.LevelNumber + 1;
        Level nextLevel = playerProgress.Levels.Find(l => l.LevelNumber == nextLevelNumber);
        if (nextLevel != null && !nextLevel.IsUnlocked)
        {
            playerProgress.Levels.Find(l => l.LevelNumber == nextLevelNumber).UnlockLevel();
        }

        playerProgress.SaveProgress();

    }


    // Save a single completed world
    public void SaveCompletedWorld ( int world )
    {
        if (world > playerProgress.WorldCompleted)
        {
            playerProgress.WorldCompleted = world;
            playerProgress.SaveProgress();
        }
    }

    // Method to be called when the player completes a level
    public void SavePlayerProgress ()
    {
        playerProgress.SaveProgress();
    }

    [ContextMenu("Reset Progress")]
    public void ResetProgress ()
    {
        playerProgress.ResetProgress();
        Debug.Log("Progress has been reset.");
    }

    public void SetCurrentLevelNumber ( int levelNumber )
    {
        currentLevel =
            playerProgress.Levels.Find(l => l.LevelNumber == levelNumber);
    }

    public void LoadLevelObjects ( int levelNumber )
    {
        currentLevelObjects =
            defaultLevels.Find(l => l.LevelNumber == levelNumber).ToriObjects;
    }



    public void OpenLevelByLevelNumber ( int levelNumber )
    {
        playerProgress.Levels.Find(l => l.LevelNumber == levelNumber).OpenLevel();
        SavePlayerProgress();
    }

    public void UnlockLevelByLevelNumber ( int levelNumber )
    {
        playerProgress.Levels.Find(l => l.LevelNumber == levelNumber).UnlockLevel();
        SavePlayerProgress();
    }

    public void CompleteLevelByLevelNumber ( int levelNumber )
    {
        playerProgress.Levels.Find(l => l.LevelNumber == levelNumber).CompleteLevel();
        SavePlayerProgress();
        Debug.Log("Level " + currentLevel.LevelNumber + " completed!");

    }

}
