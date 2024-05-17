using System.Collections.Generic;
using UnityEngine;

public class ChestsManager : MonoBehaviour
{
    [SerializeField] List<GameObject> chestPrefabs = new List<GameObject>();

    [SerializeField] private RectTransform chestParent;

    private int currentChestIndex = 0;

    private Vector3 chestPosition;

    private void Start ()
    {
        if (chestPrefabs.Count > 0)
        {
            currentChestIndex = 0;
            chestPosition = chestParent.transform.position;
            InstantiateChest(currentChestIndex);
        }
    }

    public void NextChest ()
    {
        if (chestPrefabs.Count > 0)
        {
            currentChestIndex = (currentChestIndex + 1) % chestPrefabs.Count;
            InstantiateChest(currentChestIndex);
        }
    }

    public void PreviousChest ()
    {
        if (chestPrefabs.Count > 0)
        {
            currentChestIndex = (currentChestIndex - 1 + chestPrefabs.Count) % chestPrefabs.Count;
            InstantiateChest(currentChestIndex);
        }
    }

    private void InstantiateChest ( int index )
    {
        ClearChests();
        Instantiate(chestPrefabs[index], chestPosition, Quaternion.identity, chestParent);
    }

    private void ClearChests ()
    {
        foreach (Transform child in chestParent)
        {
            Destroy(child.gameObject);
        }
    }

}
