using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Answer answer;
    [SerializeField] private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Canvas canvas;
    private bool isDraggable = true;
    private RectTransform target;

    [SerializeField] private RandomMoveUI randomMoveUI;

    private Vector2 originalPosition;
    private Vector2 dragOffset;

    void Awake ()
    {
        rectTransform = GetComponent<RectTransform>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag ( PointerEventData eventData )
    {
        if (!isDraggable) return;

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        if (randomMoveUI != null)
        {
            randomMoveUI.PauseMovement();
        }

        // Store the initial position and calculate the drag offset
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out dragOffset);
        dragOffset = rectTransform.anchoredPosition - dragOffset;
    }

    public void OnDrag ( PointerEventData eventData )
    {
        if (!isDraggable) return;

        // Convert the screen point to local point in rectangle
        Vector2 localPointerPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPointerPosition);

        // Apply the drag offset
        Vector2 newPos = localPointerPosition + dragOffset;

        // Get the canvas RectTransform to calculate bounds
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        // Calculate bounds based on the size of the canvas and the size of the draggable object
        float minX = (canvasRect.rect.width * -0.5f) + (rectTransform.rect.width * 0.5f);
        float maxX = (canvasRect.rect.width * 0.5f) - (rectTransform.rect.width * 0.5f);
        float minY = (canvasRect.rect.height * -0.5f) + (rectTransform.rect.height * 0.5f);
        float maxY = (canvasRect.rect.height * 0.5f) - (rectTransform.rect.height * 0.5f);

        // Clamp the new position within the bounds
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        // Set the new position
        rectTransform.anchoredPosition = newPos;
    }

    public void OnEndDrag ( PointerEventData eventData )
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (answer)
        {
            CheckTarget(eventData);
        }

        if (randomMoveUI != null && isDraggable)
        {
            randomMoveUI.ResumeMovement();
        }
    }

    public void SetTarget ( RectTransform target )
    {
        this.target = target;
    }

    public void CheckTarget ( PointerEventData eventData )
    {
        // Debugging output to help diagnose the issue
        Debug.Log("Checking target");

        if (target == null)
        {
            Debug.LogError("Target is not set.");
            return;
        }

        Vector2 localPointerPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPointerPosition);

        if (RectTransformUtility.RectangleContainsScreenPoint(target, eventData.position, eventData.pressEventCamera))
        {
            Debug.Log("Dropped on target");
            this.transform.SetParent(target);
            this.transform.localPosition = Vector3.zero;
            answer.PlayerAnswerCorrect();
            DisableDrag();
        }
        else
        {
            Debug.Log("Not on target");
        }
    }

    public void DisableDrag ()
    {
        isDraggable = false;
        canvasGroup.blocksRaycasts = false;

        if (randomMoveUI != null)
        {
            randomMoveUI.PauseMovement();
        }
    }

    public void EnableDrag ()
    {
        isDraggable = true;
        canvasGroup.blocksRaycasts = true;

        if (randomMoveUI != null)
        {
            randomMoveUI.ResumeMovement();
        }
    }
}
