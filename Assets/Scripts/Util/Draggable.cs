using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private bool isDraggable = true;

    [SerializeField] private ChestKey chestKey;

    void Awake ()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag ( PointerEventData eventData )
    {
        if (!isDraggable) return;

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag ( PointerEventData eventData )
    {
        if (!isDraggable) return;

        Vector2 newPos = rectTransform.anchoredPosition + eventData.delta / canvas.scaleFactor;
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        // Calculate bounds based on the size of the canvas and the size of the draggable object
        float minX = (canvasRect.rect.width * -0.5f) + (rectTransform.rect.width * 0.5f);
        float maxX = (canvasRect.rect.width * 0.5f) - (rectTransform.rect.width * 0.5f);
        float minY = (canvasRect.rect.height * -0.5f) + (rectTransform.rect.height * 0.5f);
        float maxY = (canvasRect.rect.height * 0.5f) - (rectTransform.rect.height * 0.5f);

        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        rectTransform.anchoredPosition = newPos;
    }

    public void OnEndDrag ( PointerEventData eventData )
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;


        if (chestKey)
        {
            chestKey.CheckTarget(eventData);
        }
    }

    public void DisableDrag ()
    {
        isDraggable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void EnableDrag ()
    {
        isDraggable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
