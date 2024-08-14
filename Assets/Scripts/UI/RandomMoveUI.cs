using UnityEngine;
using System.Collections;

public class RandomMoveUI : MonoBehaviour
{
    public RectTransform uiElement;
    public float moveInterval = 1.0f;
    public float maxMoveSpeed = 100f; // Maximum speed in pixels per second
    [SerializeField] private Answer answer;
    private Vector2 screenBoundsMin;
    private Vector2 screenBoundsMax;
    private Vector2 targetPosition;
    private bool isMoving;
    private bool isPaused;
    private Coroutine moveCoroutine;
    private RectTransform canvasRectTransform;
    private Camera uiCamera;

    public void Initialize ()
    {
        if (uiElement == null)
        {
            uiElement = GetComponent<RectTransform>();
        }

        canvasRectTransform = answer.GetAnswersManager().canvas.GetComponent<RectTransform>();
        uiCamera = answer.GetAnswersManager().canvas.worldCamera; // Reference to the camera used for the UI

        CalculateScreenBounds();
        moveCoroutine = StartCoroutine(MoveRandomly());
    }

    public void SetCanvas ( RectTransform _canvasRectTransform, Camera _uiCamera )
    {
        this.canvasRectTransform = _canvasRectTransform;
        this.uiCamera = _uiCamera;
        CalculateScreenBounds();
    }

    void CalculateScreenBounds ()
    {
        // Get the screen bounds in world space
        Vector3[] canvasCorners = new Vector3[4];
        canvasRectTransform.GetWorldCorners(canvasCorners);

        // Convert world space corners to screen space
        screenBoundsMin = RectTransformUtility.WorldToScreenPoint(uiCamera, canvasCorners[0]); // Bottom-left corner
        screenBoundsMax = RectTransformUtility.WorldToScreenPoint(uiCamera, canvasCorners[2]); // Top-right corner
    }

    Vector2 GetRandomPositionWithinBounds ()
    {
        float randomX = Random.Range(screenBoundsMin.x + uiElement.rect.width / 2, screenBoundsMax.x - uiElement.rect.width / 2);
        float randomY = Random.Range(screenBoundsMin.y + uiElement.rect.height / 2, screenBoundsMax.y - uiElement.rect.height / 2);

        // Convert screen space position to local position relative to the canvas
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, new Vector2(randomX, randomY), uiCamera, out localPoint);

        return localPoint;
    }

    IEnumerator MoveRandomly ()
    {
        while (true)
        {
            if (!isPaused)
            {
                if (!isMoving)
                {
                    targetPosition = GetRandomPositionWithinBounds();
                    StartCoroutine(MoveToPosition(targetPosition));
                }
                yield return new WaitForSeconds(moveInterval);
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator MoveToPosition ( Vector2 target )
    {
        isMoving = true;
        Vector2 startPosition = uiElement.anchoredPosition;
        float distance = Vector2.Distance(startPosition, target);
        float duration = distance / maxMoveSpeed;
        float elapsedTime = 0;

        while (elapsedTime < duration && !isPaused)
        {
            uiElement.anchoredPosition = Vector2.Lerp(startPosition, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (!isPaused)
        {
            uiElement.anchoredPosition = target;
        }
        isMoving = false;
    }

    public void PauseMovement ()
    {
        isPaused = true;
    }

    public void ResumeMovement ()
    {
        isPaused = false;
        if (moveCoroutine == null)
        {
            moveCoroutine = StartCoroutine(MoveRandomly());
        }
    }

    void OnDisable ()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }

        StopAllCoroutines();  // Make sure all coroutines are stopped

        if (gameObject != null)
        {
            Destroy(gameObject);  // Destroy the object, but ensure it's not being accessed afterward
        }
    }

}
