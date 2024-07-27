using UnityEngine;
using UnityEngine.UI;
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

    void Start ()
    {
        if (uiElement == null)
        {
            uiElement = GetComponent<RectTransform>();
        }
        CalculateScreenBounds();
        moveCoroutine = StartCoroutine(MoveRandomly());
    }

    void CalculateScreenBounds ()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 elementSize = uiElement.rect.size;
        screenBounds = screenSize - elementSize;
    }

    Vector2 GetRandomPositionWithinBounds ()
    {
        float randomX = Random.Range(0, screenBounds.x);
        float randomY = Random.Range(0, screenBounds.y);
        return new Vector2(randomX, randomY);
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