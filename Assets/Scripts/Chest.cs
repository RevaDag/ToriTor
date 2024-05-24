using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    [SerializeField] private Image chestLidImage;
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
    private Vector2 chestLidInitialPosition;

    private void Start ()
    {
        chestLidInitialPosition = chestLidImage.transform.position;

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

            CanvasGroup canvasGroup = keysParent.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1f;

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

    private void ResetChestLidPosition ()
    {
        StartCoroutine(MoveAndFadeLidAndFadeOutKeys(chestLidImage.rectTransform, 1.0f, false));
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
        StartCoroutine(MoveAndFadeLidAndFadeOutKeys(chestLidImage.rectTransform, 1.0f, true));
        ShowChestObject();
    }

    private IEnumerator MoveAndFadeLidAndFadeOutKeys ( RectTransform lid, float duration, bool isUp )
    {
        CanvasGroup keysCanvasGroup = keysParent.GetComponent<CanvasGroup>();
        CanvasGroup lidCanvasGroup = chestLidImage.GetComponent<CanvasGroup>();

        Vector3 startPosition = lid.localPosition;
        Vector3 endPosition;

        if (isUp)
            endPosition = new Vector3(startPosition.x, startPosition.y + moveDistance, startPosition.z);
        else
            endPosition = new Vector3(startPosition.x, startPosition.y + -moveDistance, startPosition.z);

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Move the lid
            lid.localPosition = Vector3.Lerp(startPosition, endPosition, t);

            // Fade out the keys and the lid
            float alpha;
            if (isUp)
                alpha = Mathf.Lerp(1f, 0f, t);
            else
                alpha = Mathf.Lerp(0f, 1f, t);

            keysCanvasGroup.alpha = alpha;
            lidCanvasGroup.alpha = alpha;

            yield return null;
        }
    }


    private void ShowChestObject ()
    {
        if (chestObjectPrefab == null) return;

        chestObject = Instantiate(chestObjectPrefab, transform.position, Quaternion.identity, gameObject.transform);
        chestObject.GetComponent<RectTransform>().SetAsFirstSibling();
        chestObject.GetComponent<ChestObject>().SetChest(this);
    }

    public void ReloadChest ()
    {
        UpdateKeyPrefabs();
        ResetChestLidPosition();
    }
}
