using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsBook : MonoBehaviour, IBook
{
    [SerializeField] private TMP_Text objectText;
    [SerializeField] private Image image;
    [SerializeField] private AudioSource objectWordAudioSource;
    [SerializeField] private AudioSource objectSoundAudioSource;
    [SerializeField] private Button objectImageButton;

    [SerializeField] private Fader fader;

    [SerializeField] private BookPagesController bookPagesController;

    [SerializeField] private Sprite checkIconSprite;

    [SerializeField] private QuizSummary quizSummary;

    [SerializeField] private bool isLevel;

    private List<ToriObject> objects;
    private ToriObject currentObject;

    public int bookItems { get; set; }
    public int objectsPerPage { get; set; } = 1;


    private void Start ()
    {
        if (isLevel)
        {
            GetSelectedObjectsFromGameManager();
            InitiateBook();
            SetBookPage(0);
        }
    }

    public void InitiateBook ()
    {
        bookPagesController.SetBook(this);
    }


    public void SetBookPageController ()
    {
        bookPagesController.SetBook(this);
        bookPagesController.UpdateNextAndPreviousButtons();
    }

    public void GetSelectedObjectsFromGameManager ()
    {
        objects = GameManager.Instance.selectedObjects;
        bookItems = objects.Count;
    }

    public void GetLearnedObjectsFromGameManager ()
    {
        objects = GameManager.Instance.GetLearnedObjectsBySubject
(bookPagesController.selectedLearnedSubject);
        bookItems = objects.Count;

    }

    public void SetCurrentObject ( ToriObject toriObject )
    {
        currentObject = toriObject;
        FindAndSetCurrentObjectPage();
    }

    public void FindAndSetCurrentObjectPage ()
    {
        if (currentObject == null || objects == null || objects.Count == 0)
        {
            Debug.LogWarning("Current object or objects list is null or empty.");
            return;
        }

        int currentObjectIndex = objects.IndexOf(currentObject);

        if (currentObjectIndex != -1)
        {
            SetBookPage(currentObjectIndex);
            bookPagesController.SetCurrentPage(currentObjectIndex);
        }
        else
        {
            Debug.LogWarning("Current object not found in the objects list.");
        }
    }

    public void SetBookPage ( int objectNumber )
    {
        fader.FadeOut();

        ToriObject obj = objects[objectNumber];

        objectText.text = obj.objectName;
        image.sprite = obj.sprite;
        image.SetNativeSize();
        image.color = obj.color;
        objectWordAudioSource.clip = obj.clip;

        if (obj.objectSoundClip != null)
        {
            objectSoundAudioSource.clip = obj.objectSoundClip;
            objectImageButton.interactable = true;
        }
        else
            objectImageButton.interactable = false;

        fader.FadeIn();

        SayTheWord();
    }


    public void SayTheWord ()
    {
        objectWordAudioSource.Play();
    }

    public void PlaySound ()
    {
        objectSoundAudioSource.Play();
    }

    public void Complete ()
    {
        quizSummary.ShowSummary();
    }

    public void ResetBook ()
    {
        SetBookPage(0);
        bookPagesController.ResetPages();
        quizSummary.HideSummary();
    }

    public void FadeIn ()
    {
        fader?.FadeIn();
    }

    public void FadeOut ()
    {
        fader?.FadeOut();
    }

}
