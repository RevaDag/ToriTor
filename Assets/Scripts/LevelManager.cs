using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public void LoadLevel ( string sceneName )
    {
        if (SceneFader.Instance != null)
        {
            SceneFader.Instance.LoadScene(sceneName);
        }
    }
}
