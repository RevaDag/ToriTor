using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BookManager : MonoBehaviour
{
    [SerializeField] private SwipeController swipeController;

    [SerializeField] private bool loadFromCollection;
    [SerializeField] List<CollectibleObject> objects = new List<CollectibleObject>();

    [SerializeField] private TMP_Text subjectTitle;
    [SerializeField] GameObject objectParent;
    [SerializeField] float fadeDuration = 1.0f;

    [SerializeField] private GameObject langFrame;
    [SerializeField] private GameObject hebrewButtonParent;
    [SerializeField] private GameObject englishButtonParent;

    private int currentPageIndex = 0;
    private CanvasGroup pageCanvasGroup;
    private int selectedLang = 0; // Default Hebrew

    private BookObject currentBookObject;
    private CollectibleObject currentCollectibleObject;


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

    void Start ()
    {
        pageCanvasGroup = objectParent.GetComponent<CanvasGroup>();
        if (objects.Count > 0)
        {
            DisplayPage(currentPageIndex);
            StartCoroutine(FadeIn(pageCanvasGroup));
        }

        if (loadFromCollection)
            LoadSubjectFromCollection();
    }

    private void LoadSubjectFromCollection ()
    {
        if (ObjectCollection.Instance != null)
        {
            ObjectCollection.Instance.SetBookManager(this);
            ObjectCollection.Instance.LoadSubject();
        }
    }

    public void SetObjects ( List<CollectibleObject> _objects )
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
        currentPageIndex = (currentPageIndex + 1) % objects.Count;
        StartCoroutine(ChangePage(currentPageIndex));
    }

    public void PreviousPage ()
    {
        if (currentPageIndex == 0)
        {
            currentPageIndex = objects.Count - 1;
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
