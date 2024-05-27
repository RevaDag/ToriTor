using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerProgress playerProgress { get; private set; }

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

    public void SaveOpenLevel ( int level )
    {
        if (!playerProgress.LevelsOpened.Contains(level))
        {
            playerProgress.LevelsOpened.Add(level);
            playerProgress.SaveProgress();
        }
    }

    // Save a single completed level
    public void SaveCompletedLevel ( int level )
    {
        if (!playerProgress.LevelsCompleted.Contains(level))
        {
            playerProgress.LevelsCompleted.Add(level);

            // Unlock the next level
            int nextLevel = level + 1;
            if (!playerProgress.LevelsOpened.Contains(nextLevel))
            {
                playerProgress.LevelsUnlocked.Add(nextLevel);
            }

            playerProgress.SaveProgress();
        }
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



}
