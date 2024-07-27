using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Answer answer;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private bool isDraggable = true;
    private RectTransform target;

    [SerializeField] private RandomMoveUI randomMoveUI;



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

        if (randomMoveUI != null)
        {
            randomMoveUI.PauseMovement();
        }
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
        if (RectTransformUtility.RectangleContainsScreenPoint(target, Input.mousePosition, null))
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
