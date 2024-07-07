using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookManager : MonoBehaviour
{
    [SerializeField] private TMP_Text objectText;
    [SerializeField] private Image image;
    [SerializeField] private AudioSource objectWordAudioSource;
    [SerializeField] private AudioSource objectSoundAudioSource;
    [SerializeField] private Button objectImageButton;

    [SerializeField] private Fader fader;

    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;

    [SerializeField] private Sprite checkIconSprite;
    private Sprite nextIconSprite;

    private CanvasGroup rightButtonCanvasGroup;
    private Image nextIconImage;

    private List<ToriObject> objects;
    private int currentObjectIndex;
    private void Start ()
    {
        objects = GameManager.Instance.selectedObjects;

        rightButtonCanvasGroup = previousButton.GetComponent<CanvasGroup>();

        nextIconImage = nextButton.transform.GetChild(0).GetComponent<Image>();
        nextIconSprite = nextIconImage.sprite;

        SetBookObject(objects[currentObjectIndex]);
    }

    public void LeftButtonClicked ()
    {
        if (currentObjectIndex == objects.Count - 1)
        {
            CompleteLevel();
        }
        else
        {
            NextWord();
        }

    }

    private void NextWord ()
    {
        currentObjectIndex++;
        SetBookObject(objects[currentObjectIndex]);
        UpdateNextAndPreviousButtons();

    }

    public void PreviousWord ()
    {
        currentObjectIndex--;
        SetBookObject(objects[currentObjectIndex]);
        UpdateNextAndPreviousButtons();
    }

    private void UpdateNextAndPreviousButtons ()
    {
        if (currentObjectIndex == objects.Count - 1)
        {
            nextIconImage.sprite = checkIconSprite;
            nextIconImage.SetNativeSize();
        }
        else
        {
            if (nextIconImage.sprite == checkIconSprite)
            {
                nextIconImage.sprite = nextIconSprite;
                nextIconImage.SetNativeSize();
            }
        }



        if (currentObjectIndex == 0)
        {
            rightButtonCanvasGroup.interactable = false;
            rightButtonCanvasGroup.alpha = .5f;
        }
        else
        {
            rightButtonCanvasGroup.interactable = true;
            rightButtonCanvasGroup.alpha = 1f;
        }
    }


    private void SetBookObject ( ToriObject toriObject )
    {
        fader.FadeOut();

        objectText.text = toriObject.objectName;
        image.sprite = toriObject.sprite;
        image.color = toriObject.color;
        objectWordAudioSource.clip = toriObject.clip;

        if (toriObject.objectSoundClip != null)
        {
            objectSoundAudioSource.clip = toriObject.objectSoundClip;
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

    private void CompleteLevel ()
    {
        Debug.Log("COMPLETE!");
    }

}
