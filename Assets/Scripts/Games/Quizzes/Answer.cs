using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Answer : MonoBehaviour
{
    public Image image;
    public AudioSource audioSource;
    public Draggable draggable;
    public RectTransform target;
    private bool isCorrect;
    private Vector3 initialScale;
    private Vector2 initialPosition;
    private Transform initialParent;
    private RectTransform rectTransform;

    private Fader fader;

    public ToriObject toriObject { get; private set; }
    [SerializeField] private AnswersManager answersManager;

    [Header("Additionals")]
    [SerializeField] private RandomMoveUI randomMoveUI;
    public RandomSprite randomSprite;

    public void Initialize ()
    {
        rectTransform = GetComponent<RectTransform>();
        fader = GetComponent<Fader>();

        initialParent = transform.parent;
        initialScale = rectTransform.localScale;
        initialPosition = rectTransform.anchoredPosition;

        if (randomMoveUI != null)
            randomMoveUI.Initialize();
    }

    public void SetAnswersManager ( AnswersManager _answersManager )
    {
        this.answersManager = _answersManager;
    }

    public AnswersManager GetAnswersManager ()
    {
        return answersManager;
    }

    public void SetObject ( ToriObject obj )
    {
        toriObject = obj;
    }

    public void SetAudioClip ( AudioClip _clip )
    {
        audioSource.clip = _clip;
    }

    public void SetImage ( Sprite _sprite )
    {
        image.sprite = _sprite;
        image.SetNativeSize();
    }

    public void SetColor ( Color _color )
    {
        image.color = _color;
    }

    public void SetAsCorrect () { isCorrect = true; }

    public void SetTarget ( RectTransform target )
    {
        draggable.SetTarget(target);
    }

    public void OnClickAnswer ()
    {
        PlayClip();

        if (isCorrect)
        {
            PlayerAnswerCorrect();
        }

    }

    public void PlayClip ()
    {
        audioSource.Play();

    }

    public void PlayerAnswerCorrect ()
    {
        answersManager.SetCurrentAnswer(this);
        answersManager.quizManager.CorrectAnswer(this);
    }

    public void ResetAnswer ()
    {
        isCorrect = false;
        transform.SetParent(initialParent);
        rectTransform.localScale = initialScale;
        rectTransform.anchoredPosition = initialPosition;

        if (draggable)
        {
            draggable.SetTarget(null);
            draggable.EnableDrag();
        }
    }

    public ToriObject GetToriObject ()
    {
        return toriObject;
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
