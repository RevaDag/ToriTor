using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    public enum ChestType
    {
        Shapes,
        Colors
    }

    [SerializeField] private ChestType type;

    [SerializeField] private Image chestLidImage;
    [SerializeField] private float moveDistance = 100f;
    [SerializeField] private GameObject chestObjectPrefab;

    [SerializeField] private RectTransform lockRectTransform;
    [SerializeField] private Image lockBackground;
    [SerializeField] private Transform keysParent;
    [SerializeField] GameObject[] chestKeys;
    [SerializeField] private GameObject[] keyPrefabs;
    [SerializeField] private Color[] colors;

    private Sprite suitableKeySprite;
    private Color suitableKeyColor;
    private GameObject suitableKey;
    private GameObject chestObject;
    private Vector2[] keysInitialPositions;
    private Vector2 chestLidInitialPosition;

    private void Start ()
    {
        chestLidInitialPosition = chestLidImage.transform.position;

        StoreInitialKeyPositions();
        RestartGame();
    }

    private void RestartGame ()
    {
        if (type == ChestType.Shapes)
            InstantiateKeys();

        if (type == ChestType.Colors)
            UpdateKeysColor();
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

    public void InstantiateKeys ()
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

            InitiateChestKey(chestKey);

            chestKeys[i] = keyInstance;

            CanvasGroup canvasGroup = keysParent.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1f;

            if (i == suitableKeyIndex)
                SetSuitableKeyToChest(keyInstance);
        }

        UpdateLockImage();
    }

    private void UpdateKeysColor ()
    {
        List<Color> availableColors = new List<Color>(colors);
        List<GameObject> availableKeys = new List<GameObject>(chestKeys);

        foreach (GameObject key in chestKeys)
        {
            Color randomColor = GetRandomElement(availableColors);
            availableColors.Remove(randomColor);

            ChestKey chestKey = key.GetComponentInChildren<ChestKey>();
            chestKey.keyImage.color = randomColor;
            InitiateChestKey(chestKey);
        }

        for (int i = 0; i < 3; i++)
        {
            availableKeys[i].transform.localScale = Vector3.one;
            availableKeys[i].transform.SetParent(keysParent, false);
            availableKeys[i].GetComponent<RectTransform>().anchoredPosition = keysInitialPositions[i];
        }

        GameObject correctKey = GetRandomElement(availableKeys);
        availableKeys.Remove(correctKey);

        Color correctColor = correctKey.GetComponentInChildren<ChestKey>().keyImage.color;
        lockBackground.color = correctColor;

        SetSuitableKeyToChest(correctKey);
    }

    private T GetRandomElement<T> ( List<T> list )
    {
        if (list == null || list.Count == 0)
        {
            Debug.LogWarning("The list is empty or not assigned.");
            return default(T);
        }

        int randomIndex = Random.Range(0, list.Count);
        return list[randomIndex];
    }

    private void SetSuitableKeyToChest ( GameObject keyInstance )
    {
        suitableKey = keyInstance;
        suitableKeySprite = keyInstance.GetComponentInChildren<ChestKey>().keyImage.sprite;

        ChestKey chestKey = keyInstance.GetComponentInChildren<ChestKey>();
        chestKey.SetChest(this);
        chestKey.SetTarget(lockRectTransform);
        chestObjectPrefab = chestKey.parallelObject;
    }

    private void InitiateChestKey ( ChestKey chestKey )
    {
        chestKey.SetChest(this);
        chestKey.EnableDrag();
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
        RestartGame();
        ResetChestLidPosition();
    }
}
