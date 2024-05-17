using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    [SerializeField] private Image chestLeadImage;
    [SerializeField] private float moveDistance = 100f;
    [SerializeField] private GameObject chestObjectPrefab;

    [SerializeField] private RectTransform lockRectTransform;
    [SerializeField] private Transform keysParent;
    [SerializeField] GameObject[] chestKeys;
    [SerializeField] private GameObject[] keyPrefabs;

    private Sprite suitableKeySprite;
    private GameObject suitableKey;
    private GameObject chestObject;
    private Vector2[] keysInitialPositions;

    private void Start ()
    {
        StoreInitialKeyPositions();
        UpdateKeyPrefabs();
    }

    private void StoreInitialKeyPositions ()
    {
        keysInitialPositions = new Vector2[keysParent.childCount];
        for (int i = 0; i < keysParent.childCount; i++)
        {
            RectTransform keyRectTransform = keysParent.GetChild(i).GetComponent<RectTransform>();
            if (keyRectTransform != null)
            {
                keysInitialPositions[i] = keyRectTransform.anchoredPosition;
            }
        }
    }

    public void UpdateKeyPrefabs ()
    {
        if (keyPrefabs.Length < 3)
        {
            Debug.LogError("Not enough key prefabs specified.");
            return;
        }

        // Clear previous keys
        foreach (GameObject key in chestKeys)
        {
            Destroy(key);
        }

        chestKeys = new GameObject[3];

        List<int> selectedIndices = GetRandomUniqueIndices(3, keyPrefabs.Length);
        int suitableKeyIndex = Random.Range(0, 3);

        for (int i = 0; i < 3; i++)
        {
            GameObject keyInstance = Instantiate(keyPrefabs[selectedIndices[i]], keysParent);
            RectTransform keyRectTransform = keyInstance.GetComponent<RectTransform>();
            keyRectTransform.anchoredPosition = keysInitialPositions[i];

            ChestKey chestKey = keyInstance.GetComponent<ChestKey>();

            chestKey.SetChest(this);
            chestKey.EnableDrag();
            chestKeys[i] = keyInstance;

            if (i == suitableKeyIndex)
            {
                suitableKey = keyInstance;
                suitableKeySprite = keyInstance.GetComponentInChildren<Image>().sprite;
                chestKey.SetTarget(lockRectTransform);
                chestObjectPrefab = chestKey.parallelObject;
            }
        }

        UpdateLockImage();
    }

    private List<int> GetRandomUniqueIndices ( int count, int max )
    {
        List<int> indices = new List<int>();
        while (indices.Count < count)
        {
            int randomIndex = Random.Range(0, max);
            if (!indices.Contains(randomIndex))
            {
                indices.Add(randomIndex);
            }
        }
        return indices;
    }

    private void UpdateLockImage ()
    {
        Image lockImage = lockRectTransform.GetComponent<Image>();
        lockImage.sprite = suitableKeySprite;
        lockImage.SetNativeSize();
    }

    public bool TryUnlock ( GameObject key )
    {
        if (key == suitableKey)
        {
            OpenChest();
            return true;
        }
        return false;
    }

    public void OpenChest ()
    {
        StartCoroutine(MoveLid(chestLeadImage.rectTransform, 1.0f));
        ShowChestObject();
    }

    private IEnumerator MoveLid ( RectTransform lid, float duration )
    {
        Vector3 startPosition = lid.localPosition;
        Vector3 endPosition = new Vector3(lid.localPosition.x, lid.localPosition.y + moveDistance, lid.localPosition.z);
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            lid.localPosition = Vector3.Lerp(startPosition, endPosition, elapsed / duration);
            yield return null;
        }

        lid.localPosition = endPosition;
    }

    private void ShowChestObject ()
    {
        chestObject = Instantiate(chestObjectPrefab, transform.position, Quaternion.identity, gameObject.transform);
    }

    public void ReloadChest ()
    {
        Destroy(chestObject);
        UpdateKeyPrefabs();
    }
}
