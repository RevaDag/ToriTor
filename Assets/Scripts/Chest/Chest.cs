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

    public AnswersManager answersManager;

    [SerializeField] private ChestType type;
    [SerializeField] private LevelManager levelManager;

    [SerializeField] private Image chestLidImage;
    [SerializeField] private float moveDistance = 100f;
    [SerializeField] private GameObject parallelObjectPrefab;

    [SerializeField] private RectTransform lockRectTransform;
    [SerializeField] private Image lockBackground;
    [SerializeField] private List<ToriObject> objects;

    private Sprite suitableKeySprite;
    private ChestKey suitableKey;
    private GameObject parallelObject;


    private void OnEnable ()
    {
        answersManager.OnAnswersManagerReady += OnAnswersManagerReady;
    }

    private void OnDisable ()
    {
        answersManager.OnAnswersManagerReady -= OnAnswersManagerReady;

    }

    private void OnAnswersManagerReady ()
    {
        ResetKeysAndLock();
    }


    private void ResetKeysAndLock ()
    {
        UpdateKeys();
        UpdateLockImage();
    }


    private void UpdateKeys ()
    {
        List<Answer> allAnswers = answersManager.GetAnswerList();

        foreach (Answer answer in allAnswers)
        {
            ResetKey(answer.GetComponent<ChestKey>());
        }

        ChestKey correctChestKey = answersManager.currentCorrectAnswer.GetComponent<ChestKey>();
        SetSuitableKeyToChest(correctChestKey);
    }

    private void SetSuitableKeyToChest ( ChestKey chestKey )
    {
        suitableKey = chestKey;
        suitableKeySprite = chestKey.answer.toriObject.sprite;


        chestKey.SetChest(this);
        chestKey.SetTarget(lockRectTransform);
        parallelObjectPrefab = chestKey.answer.toriObject.parallelObject;
    }


    private void UpdateLockImage ()
    {
        Image lockImage = lockRectTransform.GetComponent<Image>();
        lockImage.sprite = suitableKeySprite;
        lockImage.SetNativeSize();

        Color correctColor = answersManager.currentCorrectAnswer.toriObject.color;
        lockBackground.color = correctColor;
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
        //levelManager.NextStep();
    }

    private IEnumerator MoveAndFadeLidAndFadeOutKeys ( RectTransform lid, float duration, bool isUp )
    {
        CanvasGroup keysCanvasGroup = answersManager.GetComponent<CanvasGroup>();
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

    public void PositiveFeedbackRequest ()
    {
        answersManager.feedbackManager.SendFeedback(0);
    }

    public void ReloadChest ()
    {
        ResetChestLidPosition();
        answersManager.SetAnswers();

        ResetKeysAndLock();
    }

    private void ResetKey ( ChestKey key )
    {
        key.SetTarget(null);
        key.EnableDrag();
        suitableKey = null;
        suitableKeySprite = null;
    }
}
