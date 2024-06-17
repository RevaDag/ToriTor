using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Answer : MonoBehaviour
{
    public Image image;
    public AudioSource audioSource;
    public Draggable draggable;
    private bool isCorrect;
    private Vector3 initialScale;
    private Vector2 initialPosition;
    private Transform initialParent;
    private RectTransform rectTransform;


    public ToriObject toriObject { get; private set; }
    public AnswersManager answersManager { get; private set; }

    private void Awake ()
    {
        rectTransform = GetComponent<RectTransform>();

        initialParent = transform.parent;
        initialScale = rectTransform.localScale;
        initialPosition = rectTransform.anchoredPosition;
    }

    public void SetAnswer ( ToriObject _toriObject )
    {
        toriObject = _toriObject;
        image.sprite = _toriObject.sprite;
        image.color = _toriObject.color;
        audioSource.clip = _toriObject.clip;
    }

    public void SetAnswersManager ( AnswersManager _answersManager )
    {
        if (answersManager == null)
            answersManager = _answersManager;
    }

    public void SetAsCorrect () { isCorrect = true; }

    public void OnClickAnswer ()
    {
        audioSource.Play();

        if (isCorrect)
        {
            answersManager.CorrectAnswer();
            Debug.Log("CORRECT!");
        }
        else
        {
            answersManager.WrongAnswer();
            Debug.Log("WRONG!");
        }
    }

    public void ResetAnswer ()
    {
        isCorrect = false;
        transform.SetParent(initialParent);
        rectTransform.localScale = initialScale;
        rectTransform.anchoredPosition = initialPosition;
    }

}
