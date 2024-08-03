using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<Fader> levelFaders;
    [SerializeField] private List<GameObject> levelStars;


    void Start ()
    {
        FadeOutAllLevels();
        DeactivateAllStars();

        _ = LoadingScreen.Instance.HideLoadingScreen();
        _ = InitiateMap();
    }

    private async Task InitiateMap ()
    {
        await Task.Delay(3000);
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

    private void FadeOutAllLevels ()
    {
        foreach (var levelFader in levelFaders)
        {
            levelFader.FadeOut();
        }
    }

    private void UpdateStars ()
    {
        int currentLevel = GameManager.Instance.currentLevel;

        for (int i = 0; i < levelStars.Count - 1; i++)
        {
            if (i < currentLevel)
            {
                levelStars[i].SetActive(true);
            }
        }
    }

    public async Task UpdateMap ()
    {
        int currentLevel = GameManager.Instance.currentLevel;

        for (int i = 0; i < levelFaders.Count; i++)
        {
            if (i <= currentLevel)
            {
                levelFaders[i].FadeIn();
                await Task.Delay(500);
            }
        }
    }

    public void ResetMap ()
    {
        GameManager.Instance.ResetLevel();
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
}
