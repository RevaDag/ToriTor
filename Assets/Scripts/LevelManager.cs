using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int world;
    private Level level;

    private void Start ()
    {
        level = GameManager.Instance.currentLevel;
    }

    public void CompleteLevel ()
    {
        GameManager.Instance.SaveCompletedLevel(level);
    }
}
