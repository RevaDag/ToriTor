using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject[] levelObjects;


    void Start ()
    {
        UpdateMap();
    }

    public void UpdateMap ()
    {
        int currentLevel = GameManager.Instance.currentLevel;

        for (int i = 0; i < levelObjects.Length; i++)
        {
            if (i <= currentLevel)
            {
                levelObjects[i].SetActive(true);
            }
            else
            {
                levelObjects[i].SetActive(false);
            }
        }
    }

    public void ResetMap ()
    {
        GameManager.Instance.ResetLevel();
    }
}
