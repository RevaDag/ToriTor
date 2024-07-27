using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookPagesController : MonoBehaviour
{

    public enum BookType
    {
        Subjects,
        Stickers,
        Objects
    }

    private BookType currentBookType;

    private int currentPage;

    private IBook book;

    [SerializeField] private SceneLoader sceneLoader;

    [SerializeField] private SubjectStickerBook stickerSubjectBook;
    [SerializeField] private ObjectStickerBook stickerObjectBook;
    [SerializeField] private ObjectsBook objectsBook;
    public LearnedSubject selectedLearnedSubject { get; private set; }


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

    public void SetSelectedSubject ( LearnedSubject subject )
    {
        selectedLearnedSubject = subject;
    }

    public void SetSelectedObject ( ToriObject toriObject )
    {
        objectsBook.SetCurrentObject(toriObject);
    }

    public void SetCurrentPage ( int pageIndex )
    {
        currentPage = pageIndex;
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


    public void ShowBook ( BookType bookType )
    {
        book.FadeOut();

        currentBookType = bookType;

        switch (bookType)
        {
            case BookType.Subjects:
                book = stickerSubjectBook;
                break;

            case BookType.Stickers:
                book = stickerObjectBook;
                objectsBook.GetLearnedObjectsFromGameManager();
                break;

            case BookType.Objects:
                book = objectsBook;
                break;
        }


        book.FadeIn();
        book.InitiateBook();
        book.SetBookPageController();
    }

    public void OnRedPreviousButtonClicked ()
    {
        switch (currentBookType)
        {
            case BookType.Subjects:
                sceneLoader.LoadPreviousScene();
                break;

            case BookType.Stickers:
                ShowBook(BookType.Subjects);
                break;

            case BookType.Objects:
                ShowBook(BookType.Stickers);
                break;
        }

    }
}
