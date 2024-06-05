using UnityEngine;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class PlayerProgress
{
    public int WorldCompleted; // Will be changes to a list of World classes
    public List<Level> Levels;

    private static string SaveFilePath => Path.Combine(Application.persistentDataPath, "playerProgress.json");

    // Load the player's progress from a JSON file
    public void LoadProgress ()
    {
        if (File.Exists(SaveFilePath))
        {
            string json = File.ReadAllText(SaveFilePath);
            JsonUtility.FromJsonOverwrite(json, this);
        }
        else
        {
            // Set default values if no save file exists
            InitializeDefaultValues();
        }
    }

    // Save the player's progress to a JSON file
    public void SaveProgress ()
    {
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(SaveFilePath, json);
        Debug.Log("Progress saved: " + json);
    }


    // Reset the player's progress
    public void ResetProgress ()
    {
        InitializeDefaultValues();

        SaveProgress();
        Debug.Log("Progress has been reset.");
    }

    private void InitializeDefaultValues ()
    {
        WorldCompleted = 0;
        Levels = GameManager.Instance.defaultLevels;
    }
}
