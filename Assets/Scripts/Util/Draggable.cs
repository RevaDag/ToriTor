using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Answer answer;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private bool isDraggable = true;
    private RectTransform target;

    [SerializeField] private RandomMoveUI randomMoveUI;
    [SerializeField] private ShapeHandler shapeHandler; // Reference to the ShapeHandler script
    [SerializeField] private LineRendererHandler lineRendererHandler; // Reference to the LineRendererHandler script


    private Vector2 dragOffset;
    private Vector3[] shapePoints;

    void Awake ()
    {
        rectTransform = GetComponent<RectTransform>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();

        if (shapeHandler != null)
        {
            shapeHandler.Initialize();
            shapePoints = shapeHandler.GetShapePoints();
        }
        else
        {
            Debug.LogWarning("ShapeHandler is not assigned.");
        }
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

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out dragOffset);
        dragOffset = rectTransform.anchoredPosition - dragOffset;


        if (lineRendererHandler != null)
            lineRendererHandler.StartLine(rectTransform.position);
    }

    public void OnDrag ( PointerEventData eventData )
    {
        if (!isDraggable) return;

        // Check if the pointer is still over the draggable object
        if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, eventData.position, eventData.pressEventCamera))
        {
            return; // Stop dragging if the pointer is not over the object
        }

        // Use OnShapeDrag if the shape is set
        if (shapeHandler != null)
        {
            OnShapeDrag(eventData);
        }
        else
        {
            OnFreeDrag(eventData);
        }
    }

    private void OnShapeDrag ( PointerEventData eventData )
    {
        Vector2 localPointerPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPointerPosition);

        Vector2 newPos = localPointerPosition + dragOffset;
        Vector3 closestPoint = shapeHandler.GetClosestPoint(newPos);

        Vector2 canvasPosition = WorldToCanvasPosition(canvas, closestPoint);

        // Set the new position constrained to the shape stroke
        rectTransform.anchoredPosition = canvasPosition;

        if (lineRendererHandler != null)
            lineRendererHandler.AddPoint(rectTransform.position);

    }

    private void OnFreeDrag ( PointerEventData eventData )
    {
        Vector2 localPointerPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPointerPosition);

        Vector2 newPos = localPointerPosition + dragOffset;

        // Clamp the position within the bounds
        rectTransform.anchoredPosition = ClampToCanvasBounds(newPos);
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

    private Vector2 WorldToCanvasPosition ( Canvas canvas, Vector3 worldPosition )
    {
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPosition, canvas.worldCamera, out Vector2 canvasPosition);
        return canvasPosition;
    }

    private Vector2 ClampToCanvasBounds ( Vector2 newPos )
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        float minX = (canvasRect.rect.width * -0.5f) + (rectTransform.rect.width * 0.5f);
        float maxX = (canvasRect.rect.width * 0.5f) - (rectTransform.rect.width * 0.5f);
        float minY = (canvasRect.rect.height * -0.5f) + (rectTransform.rect.height * 0.5f);
        float maxY = (canvasRect.rect.height * 0.5f) - (rectTransform.rect.height * 0.5f);

        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        return newPos;
    }

    public void SetTarget ( RectTransform target )
    {
        this.target = target;
    }

    public void CheckTarget ( PointerEventData eventData )
    {
        if (target == null)
        {
            Debug.LogError("Target is not set.");
            return;
        }

        if (RectTransformUtility.RectangleContainsScreenPoint(target, eventData.position, eventData.pressEventCamera))
        {
            this.transform.SetParent(target);
            this.transform.localPosition = Vector3.zero;
            answer.PlayerAnswerCorrect();
            DisableDrag();
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
