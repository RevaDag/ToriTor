using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void LoadScene ( string sceneName )
    {
        LoadingScreen.Instance.LoadScene(sceneName);
    }


    public void LoadPreviousScene ()
    {
        MusicManager.Instance.PlayAudioClip(MusicManager.Instance.mainMusic);
        LoadingScreen.Instance.LoadPreviousScene();
    }
}