using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void LoadScene ( string sceneName )
    {
        LoadSceneAndHideLoadingScreen(sceneName);
    }

    public void LoadSceneAndHideLoadingScreen ( string sceneName, float hideTime = -1f )
    {
        LoadingScreen.Instance.LoadScene(sceneName, hideTime);
    }


    public void LoadPreviousScene ()
    {
        LoadingScreen.Instance.LoadPreviousScene();
    }
}