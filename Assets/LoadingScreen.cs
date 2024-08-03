using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance { get; private set; }
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Fader fader;
    [SerializeField] private TMP_Text loadingText;

    [SerializeField] private List<string> loadingTexts;

    private string previousScene;



    private void Awake ()
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
    }

    public void LoadScene ( string sceneName )
    {
        previousScene = SceneManager.GetActiveScene().name;

        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine ( string sceneName )
    {
        SetRandomLoadingText();
        ShowLoadingScreen();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }


    public void LoadPreviousScene ()
    {
        LoadScene(previousScene);
    }

    private void SetRandomLoadingText ()
    {
        if (loadingTexts != null && loadingTexts.Count > 0)
        {
            int randomIndex = Random.Range(0, loadingTexts.Count);
            string randomText = loadingTexts[randomIndex];
            loadingText.text = randomText;
        }
        else
        {
            Debug.LogWarning("Loading texts list is empty or null.");
        }
    }

    #region Show & Hide


    public void ShowLoadingScreen ()
    {
        fader.FadeIn();
    }

    public async Task HideLoadingScreen ()
    {
        await Task.Delay(2000);
        fader.FadeOut();
    }

    #endregion
}
