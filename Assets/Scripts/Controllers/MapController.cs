using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<Fader> levelFaders;
    [SerializeField] private List<GameObject> levelStars;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private MapSummary mapSummary;


    void Start ()
    {
        HideAllLevels();
        DeactivateAllStars();

        LoadingScreen.Instance.HideLoadingScreen();
        _ = InitiateMap();
    }


    private async Task InitiateMap ()
    {
        await Task.Delay(1000);
        UpdateStars();
        _ = UpdateMap();
    }

    private void DeactivateAllStars ()
    {
        foreach (GameObject levelStar in levelStars)
        {
            levelStar.SetActive(false);
        }
    }

    private void HideAllLevels ()
    {
        foreach (Fader level in levelFaders)
        {
            level.HideObject();
        }
    }


    private void UpdateStars ()
    {
        int currentLevel = GameManager.Instance.nextLevel;

        for (int i = 0; i < levelStars.Count; i++)
        {
            // If its the previous level
            if (i < currentLevel - 1)
            {
                levelStars[i].SetActive(true);
            }
        }
    }

    public async Task UpdateMap ()
    {
        int currentLevel = GameManager.Instance.nextLevel;

        for (int i = 0; i < levelFaders.Count; i++)
        {
            if (i < currentLevel)
            {
                levelFaders[i].FadeIn();
                await Task.Delay(500);
            }
        }

        Debug.Log(currentLevel);

        if (currentLevel >= 5)
        {
            Debug.Log("Current level is 5 or higher, calling PlayMapSummary()");
            mapSummary.PlayMapSummary();
        }
    }

    public void ResetMap ()
    {
        GameManager.Instance.ResetLevel();
    }

    public void SetSelectedLevel ( int _levelNumber )
    {
        if (GameManager.Instance != null)
            GameManager.Instance.SetCurrentLevel(_levelNumber);
    }

    public void UnlockAllLevels ()
    {
        _ = UnlockAllLevelsAsync();
    }

    private async Task UnlockAllLevelsAsync ()
    {
        for (int i = 0; i < levelFaders.Count; i++)
        {
            levelFaders[i].FadeIn();
            await Task.Delay(500);
        }
    }

    public void LoadMainScene ()
    {
        sceneLoader.LoadSceneAndHideLoadingScreen("PirateIsland", 2);
    }
}
