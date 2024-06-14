using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BookManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;

    [SerializeField] private SwipeController swipeController;

    [SerializeField] private bool loadFromCollection;
    [SerializeField] List<ToriObject> objects = new List<ToriObject>();

    [SerializeField] private TMP_Text subjectTitle;
    [SerializeField] GameObject objectParent;
    [SerializeField] float fadeDuration = 1.0f;

    [SerializeField] private GameObject langFrame;
    [SerializeField] private GameObject hebrewButtonParent;
    [SerializeField] private GameObject englishButtonParent;

    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;

    private Sprite leftArrowSprite;
    [SerializeField] private Sprite checkSprite;

    private Image previousArrowImage;
    private Image nextArrowImage;
    private Color transparentWhiteColor = new Color(1f, 1f, 1f, 0.5f);


    private int currentPageIndex = 0;
    private CanvasGroup pageCanvasGroup;
    private int selectedLang = 0; // Default Hebrew

    private BookObject currentBookObject;
    private ToriObject currentCollectibleObject;


    private void OnEnable ()
    {
        swipeController.OnSwipeLeft += HandleSwipeLeft;
        swipeController.OnSwipeRight += HandleSwipeRight;
    }

    private void OnDisable ()
    {
        swipeController.OnSwipeLeft -= HandleSwipeLeft;
        swipeController.OnSwipeRight -= HandleSwipeRight;
    }

    private void Awake ()
    {
        previousArrowImage = previousButton.transform.GetChild(0).GetComponent<Image>();
        nextArrowImage = nextButton.transform.GetChild(0).GetComponent<Image>();
        leftArrowSprite = nextArrowImage.sprite;
    }

    void Start ()
    {
        pageCanvasGroup = objectParent.GetComponent<CanvasGroup>();

        if (loadFromCollection)
            LoadSubjectFromCollection();
        else
            LoadLevelObjects();

        DisplayPage(currentPageIndex);
        StartCoroutine(FadeIn(pageCanvasGroup));
    }

    private void LoadLevelObjects ()
    {
        objects?.Clear();
        objects = GameManager.Instance.currentLevelObjects;

    }

    public void LoadSubjectFromCollection ()
    {
        if (ObjectCollection.Instance != null)
        {
            ObjectCollection.Instance.SetBookManager(this);
            ObjectCollection.Instance.LoadSubjectFromCollection();
        }
    }

    public void SetObjects ( List<ToriObject> _objects )
    {
        objects = _objects;

        currentPageIndex = 0;

        foreach (Transform child in objectParent.transform)
        {
            Destroy(child.gameObject);
        }

        if (objects.Count > 0)
        {
            DisplayPage(currentPageIndex);
            StartCoroutine(FadeIn(pageCanvasGroup));
        }
    }

    public void SetSubjectTitle ( string _subject )
    {
        subjectTitle.text = _subject;
    }

    private void HandleSwipeLeft ()
    {
        PreviousPage();
    }

    private void HandleSwipeRight ()
    {
        NextPage();
    }

    public void NextPage ()
    {
        if (currentPageIndex == objects.Count - 1) return;

        currentPageIndex++;
        StartCoroutine(ChangePage(currentPageIndex));
    }

    public void PreviousPage ()
    {
        if (currentPageIndex == 0)
        {
            return;
        }
        else
        {
            currentPageIndex -= 1;
        }
        StartCoroutine(ChangePage(currentPageIndex));
    }

    private IEnumerator ChangePage ( int newIndex )
    {
        yield return StartCoroutine(FadeOut(pageCanvasGroup)); // Fade out current page
        currentPageIndex = newIndex;
        DisplayPage(currentPageIndex);
        yield return StartCoroutine(FadeIn(pageCanvasGroup)); // Fade in new page
    }

    private void DisplayPage ( int index )
    {
        ClearPageContents();

        currentCollectibleObject = objects[index];
        GameObject currentObject = objects[index].objectPrefab;
        foreach (Transform child in objectParent.transform)
        {
            Destroy(child.gameObject);
        }

        currentBookObject = Instantiate(currentObject, objectParent.transform).GetComponent<BookObject>();

        SelectLanguage(selectedLang);
        AddObjectToCollection();

        UpdatePreviousAndNextButtons();
    }

    private void UpdateButtonState ( Button button, Image arrowImage, bool isActive )
    {
        button.interactable = isActive;
        arrowImage.color = isActive ? Color.white : transparentWhiteColor;
    }

    private void UpdatePreviousAndNextButtons ()
    {
        bool isAtFirstPage = currentPageIndex == 0;
        bool isAtLastPage = currentPageIndex == objects.Count - 1;

        UpdateButtonState(previousButton, previousArrowImage, !isAtFirstPage);

        if (isAtLastPage)
        {
            if (loadFromCollection)
            {
                nextArrowImage.sprite = leftArrowSprite;
                UpdateButtonState(nextButton, nextArrowImage, !isAtLastPage);
            }
            else
            {
                nextArrowImage.sprite = checkSprite;
                nextButton.onClick.RemoveAllListeners();
                nextButton.onClick.AddListener(levelManager.CompleteLevel);
            }
        }

        else
        {
            nextArrowImage.sprite = leftArrowSprite;
            nextButton.onClick.RemoveAllListeners();
            UpdateButtonState(nextButton, nextArrowImage, !isAtLastPage);
        }

    }

    private IEnumerator FadeIn ( CanvasGroup canvasGroup )
    {
        float counter = 0f;
        while (counter < fadeDuration)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, counter / fadeDuration);
            yield return null;
        }
    }

    private IEnumerator FadeOut ( CanvasGroup canvasGroup )
    {
        float counter = 0f;
        while (counter < fadeDuration)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, counter / fadeDuration);
            yield return null;
        }
    }

    private void ClearPageContents ()
    {
        // Remove all contents in the page
        foreach (Transform child in objectParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SelectLanguage ( int languageNumber )
    {
        currentBookObject?.ChangeLanguage(languageNumber);

        switch (languageNumber)
        {
            case 0: // Hebrew
                langFrame.transform.SetParent(hebrewButtonParent.transform, false);
                break;

            case 1: // English
                langFrame.transform.SetParent(englishButtonParent.transform, false);
                break;
        }

        langFrame.transform.SetSiblingIndex(0);
        RectTransform rectTransform = langFrame.GetComponent<RectTransform>();
        rectTransform.localPosition = Vector2.zero;

        selectedLang = languageNumber;
    }

    public void SayTheWord ()
    {
        currentBookObject?.SayTheWord();
    }

    public void AddObjectToCollection ()
    {
        if (ObjectCollection.Instance != null)
        {
            ObjectCollection.Instance.AddObject(currentCollectibleObject);
        }
    }



}
