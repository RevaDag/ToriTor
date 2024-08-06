using UnityEngine;
using System.Collections;

public class RandomMoveUI : MonoBehaviour
{
    public RectTransform uiElement;
    public float moveInterval = 1.0f;
    public float maxMoveSpeed = 100f; // Maximum speed in pixels per second
    private Vector2 screenBounds;
    private Vector2 targetPosition;
    private bool isMoving;
    private bool isPaused;
    private Coroutine moveCoroutine;
    private RectTransform canvasRectTransform;

    void Start ()
    {
        if (uiElement == null)
        {
            uiElement = GetComponent<RectTransform>();
        }

        canvasRectTransform = uiElement.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        CalculateScreenBounds();
        moveCoroutine = StartCoroutine(MoveRandomly());
    }

    void CalculateScreenBounds ()
    {
        Vector3[] canvasCorners = new Vector3[4];
        canvasRectTransform.GetWorldCorners(canvasCorners);
        Vector2 bottomLeftCorner = RectTransformUtility.WorldToScreenPoint(null, canvasCorners[0]);
        Vector2 topRightCorner = RectTransformUtility.WorldToScreenPoint(null, canvasCorners[2]);
        screenBounds = topRightCorner - bottomLeftCorner;
    }

    Vector2 GetRandomPositionWithinBounds ()
    {
        Vector3[] canvasCorners = new Vector3[4];
        canvasRectTransform.GetWorldCorners(canvasCorners);
        Vector2 bottomLeftCorner = RectTransformUtility.WorldToScreenPoint(null, canvasCorners[0]);
        Vector2 topRightCorner = RectTransformUtility.WorldToScreenPoint(null, canvasCorners[2]);

        float randomX = Random.Range(bottomLeftCorner.x + uiElement.rect.width / 2, topRightCorner.x - uiElement.rect.width / 2);
        float randomY = Random.Range(bottomLeftCorner.y + uiElement.rect.height / 2, topRightCorner.y - uiElement.rect.height / 2);

        return RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRectTransform, new Vector2(randomX, randomY), null, out Vector3 worldPoint) ? worldPoint : Vector2.zero;
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
        Vector2 startPosition = uiElement.position;
        float distance = Vector2.Distance(startPosition, target);
        float duration = distance / maxMoveSpeed;
        float elapsedTime = 0;

        while (elapsedTime < duration && !isPaused)
        {
            uiElement.position = Vector2.Lerp(startPosition, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (!isPaused)
        {
            uiElement.position = target;
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
        StopAllCoroutines();
        moveCoroutine = null;
    }
}
