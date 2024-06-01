using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int world;
    [SerializeField] private int level;

    public void CompleteLevel ()
    {
        GameManager.Instance.SaveCompletedLevel(level);
    }
}
