using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance { get; private set; }
    [SerializeField] private Fader fader;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private VideoPlayer videoPlayer;

    [SerializeField] private List<string> loadingTexts;

    private string previousScene;
    private int loadingTextIndex = 0;



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
    public void LoadScene ( string sceneName, float hideTime = -1f )
    {
        previousScene = SceneManager.GetActiveScene().name;

        StartCoroutine(LoadSceneCoroutine(sceneName, hideTime));
    }

    private IEnumerator LoadSceneCoroutine ( string sceneName, float hideTime )
    {
        SetLoadingText();
        ShowLoadingScreen();

        videoPlayer.time = 0;
        videoPlayer.Play();

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(sceneName);

        if (hideTime > 0)
        {
            yield return new WaitForSeconds(hideTime);
        }
        HideLoadingScreen();

    }

    public void LoadPreviousScene ()
    {
        LoadScene(previousScene);
    }

    private void SetLoadingText ()
    {
        if (loadingTexts != null && loadingTexts.Count > 0)
        {
            string textToDisplay = loadingTexts[loadingTextIndex];
            loadingText.text = textToDisplay;
            loadingTextIndex = (loadingTextIndex + 1) % loadingTexts.Count; // Increment the index and reset if it reaches the end
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

    public void HideLoadingScreen ()
    {
        fader.FadeOut();
    }

    #endregion
}
