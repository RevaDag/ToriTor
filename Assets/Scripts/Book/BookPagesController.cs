using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookPagesController : MonoBehaviour
{

    private int currentPage;

    private IBook book;

    [SerializeField] private bool isStickerBook;

    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;

    private Sprite nextIconSprite;


    [SerializeField] private CanvasGroup rightButtonCanvasGroup;
    [SerializeField] private CanvasGroup leftButtonCanvasGroup;

    private Image nextIconImage;

    [SerializeField] private Sprite checkIconSprite;

    private void Start ()
    {
        rightButtonCanvasGroup = previousButton.GetComponent<CanvasGroup>();

        nextIconImage = nextButton.transform.GetChild(0).GetComponent<Image>();
        nextIconSprite = nextIconImage.sprite;
    }

    public void SetBook ( IBook book )
    {
        this.book = book;
    }


    public void LeftButtonClicked ()
    {
        if (currentPage == book.bookItems - 1)
        {
            book.Complete();
        }
        else
        {
            NextPage();
        }

    }

    public void NextPage ()
    {
        currentPage++;
        book.SetBookPage(currentPage);
        UpdateNextAndPreviousButtons();

    }

    public void PreviousPage ()
    {
        currentPage--;
        book.SetBookPage(currentPage);
        UpdateNextAndPreviousButtons();
    }

    public void ResetPages ()
    {
        currentPage = 0;
        UpdateNextAndPreviousButtons();
    }

    public void UpdateNextAndPreviousButtons ()
    {
        int totalPages = 0;

        if (isStickerBook)
            totalPages = Mathf.CeilToInt((float)book.bookItems / book.objectsPerPage);
        else
            totalPages = book.bookItems;

        bool isFirstPage = currentPage == 0;
        bool isLastPage = currentPage == totalPages - 1;

        UpdateButtonState(rightButtonCanvasGroup, !isFirstPage);
        UpdateButtonState(leftButtonCanvasGroup, !isLastPage || !isStickerBook);

        if (isStickerBook)
        {
            return;
        }

        nextIconImage.sprite = isLastPage ? checkIconSprite : nextIconSprite;
        nextIconImage.SetNativeSize();
    }

    private void UpdateButtonState ( CanvasGroup canvasGroup, bool isActive )
    {
        canvasGroup.interactable = isActive;
        canvasGroup.alpha = isActive ? 1f : 0.5f;
    }

}
