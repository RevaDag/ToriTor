using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Answer : MonoBehaviour
{
    public Image image;
    public AudioSource audioSource;
    private bool isCorrect;
    private Vector3 initialScale;
    private Vector2 initialPosition;
    private Transform initialParent;

    public ToriObject toriObject { get; private set; }
    private AnswersManager answersManager;

    private void Awake ()
    {
        initialPosition = transform.position;
        initialScale = transform.localScale;
        initialParent = transform.parent;
    }

    public void SetAnswer ( ToriObject _toriObject )
    {
        toriObject = _toriObject;
        image.sprite = _toriObject.sprite;
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
            //FEEDBACK - TBD
            answersManager.NextAnswer();
            Debug.Log("CORRECT!");
        }
    }

    public void ResetAnswer ()
    {
        isCorrect = false;
        transform.localScale = initialScale;
        transform.position = initialPosition;
        transform.SetParent(initialParent);
    }

}
