using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{

    public void LoadScene ( string sceneName )
    {
        if (SceneFader.Instance != null)
        {
            SceneFader.Instance.LoadScene(sceneName);
        }
    }


    public void LoadPreviousScene ()
    {
        if (SceneFader.Instance != null)
        {
            LoadScene(SceneFader.Instance.previousScene);
        }
    }
}
