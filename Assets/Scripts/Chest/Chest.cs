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
    [SerializeField] private LevelManager levelManager;

    [SerializeField] private Image chestLidImage;
    [SerializeField] private float moveDistance = 100f;
    [SerializeField] private GameObject parallelObjectPrefab;

    [SerializeField] private RectTransform lockRectTransform;
    [SerializeField] private Image lockBackground;
    [SerializeField] private Transform keysParent;
    [SerializeField] GameObject[] chestKeys;
    [SerializeField] private List<ToriObject> objects;

    private Sprite suitableKeySprite;
    private GameObject suitableKey;
    private GameObject parallelObject;
    private Vector2[] keysInitialPositions;
    private int currentSuitableKeyIndex = 0;


    private void Start ()
    {
        StoreInitialKeyPositions();
        LoadLevelObjects();
        RestartGame();
    }

    private void LoadLevelObjects ()
    {
        objects?.Clear();
        objects = GameManager.Instance.currentLevelObjects;
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


    //Needs a refactor -
    //use exsiting gameobjects as keys and change their params

    public void InstantiateKeys ()
    {
        // Clear previous keys
        foreach (GameObject key in chestKeys)
        {
            Destroy(key);
        }

        chestKeys = new GameObject[3];

        List<int> selectedIndices = GetRandomUniqueIndices(3, objects.Count);
        //int suitableKeyIndex = Random.Range(0, 3);
        int suitableKeyIndex = currentSuitableKeyIndex % 3; // Use the currentSuitableKeyIndex to determine the suitable key


        for (int i = 0; i < 3; i++)
        {
            GameObject keyInstance = Instantiate(objects[selectedIndices[i]].objectPrefab, keysParent);
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

        currentSuitableKeyIndex++;
    }

    private void UpdateKeysColor ()
    {
        List<ToriObject> availableColors = new List<ToriObject>(objects);
        List<GameObject> availableKeys = new List<GameObject>(chestKeys);

        for (int i = 0; i < chestKeys.Length; i++)
        {
            ChestKey chestKey = chestKeys[i].GetComponentInChildren<ChestKey>();
            ResetKey(chestKey);

            if (i < availableColors.Count)
            {
                ToriObject colorObject = availableColors[i];

                chestKey.keyImage.color = colorObject.color;
                chestKey.SetParallelObject(colorObject.parallelObject);
                chestKey.SetAudioClip(colorObject.clip);
                InitiateChestKey(chestKey);
            }
        }

        for (int i = 0; i < chestKeys.Length; i++)
        {
            availableKeys[i].transform.localScale = Vector3.one;
            availableKeys[i].transform.SetParent(keysParent, false);
            availableKeys[i].GetComponent<RectTransform>().anchoredPosition = keysInitialPositions[i];
        }

        // Determine the suitable key based on the currentSuitableKeyIndex
        GameObject correctKey = chestKeys[currentSuitableKeyIndex % chestKeys.Length];
        Color correctColor = correctKey.GetComponentInChildren<ChestKey>().keyImage.color;
        lockBackground.color = correctColor;

        SetSuitableKeyToChest(correctKey);

        // Increment the currentSuitableKeyIndex for the next suitable key
        currentSuitableKeyIndex++;
    }

    private void SetSuitableKeyToChest ( GameObject keyInstance )
    {
        suitableKey = keyInstance;
        suitableKeySprite = keyInstance.GetComponentInChildren<ChestKey>().keyImage.sprite;

        ChestKey chestKey = keyInstance.GetComponentInChildren<ChestKey>();
        chestKey.SetChest(this);
        chestKey.SetTarget(lockRectTransform);
        parallelObjectPrefab = chestKey.parallelObject;
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
        levelManager.NextStep();
        //chestOpenCounter++;
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
        if (parallelObjectPrefab == null) return;

        parallelObject = Instantiate(parallelObjectPrefab, transform.position, Quaternion.identity, gameObject.transform);
        parallelObject.GetComponent<RectTransform>().SetAsFirstSibling();
        parallelObject.GetComponent<ChestObject>().SetChest(this);
    }

    public void ReloadChest ()
    {
        ResetChestLidPosition();

        if (levelManager.stepper.currentStep >= GameManager.Instance.currentLevel.StepsNumber)
        {
            levelManager.CompleteLevel();
            return;
        }

        RestartGame();
    }

    private void ResetKey ( ChestKey key )
    {
        key.SetTarget(null);
        suitableKey = null;
        suitableKeySprite = null;
    }
}
