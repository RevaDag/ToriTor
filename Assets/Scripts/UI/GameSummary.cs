using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSummary : MonoBehaviour
{
    //private int levelNumber;

    private IGame gameInterface;
    [SerializeField] private QuizTester quizTester;

    [SerializeField] private Fader summaryCanvasFader;
    [SerializeField] private Sticker stickerPrefab;
    [SerializeField] private Transform stickersParent;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private List<Sticker> stickers;
    [SerializeField] private List<ParticleSystem> starConfetties;

    [SerializeField] private List<ToriObject> predefinedObjects;

    [Header("Audio")]
    [SerializeField] private AudioSource buttonsSFX;
    [SerializeField] private AudioClip success;
    private AudioSource audioSource;

    private void Awake ()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start ()
    {
        SetStickers();
    }

    public void SetGameInterface ( IGame _gameInterface )
    {
        this.gameInterface = _gameInterface;
    }

    public void SetStickers ()
    {
        List<ToriObject> toriObjects = new List<ToriObject>();
        toriObjects = GetObjects();

        for (int i = 0; i < toriObjects.Count; i++)
        {
            Sticker sticker = stickers[i];

            sticker.SetImage(toriObjects[i].sprite);
            sticker.SetAudio(toriObjects[i].clip);
            sticker.SetColor(toriObjects[i].color);
        }
    }

    private List<ToriObject> GetObjects ()
    {
        List<ToriObject> toriObjects = new List<ToriObject>();

        if (predefinedObjects.Count == 0)
        {
            // Check if quizTester exists and is in test mode
            if (quizTester != null && quizTester.isTest)
                toriObjects = quizTester.selectedObjects;

            if (SubjectsManager.Instance != null)
            {
                int levelNumber = GameManager.Instance.currentlevel;
                toriObjects = SubjectsManager.Instance.GetObjectsByListNumber(levelNumber);
            }
            else
                Debug.LogWarning("SubjectsManager.Instance is null.");
        }
        else
            toriObjects = predefinedObjects;

        return toriObjects;
    }

    private void PlayStarConfetties ()
    {
        foreach (ParticleSystem stars in starConfetties)
        {
            stars.Play();
        }
    }

    private void PlaySuccessAudioClip ()
    {
        audioSource.clip = success;
        audioSource.Play();
    }

    public void ShowSummary ()
    {
        summaryCanvasFader.FadeIn();
        PlaySuccessAudioClip();
        PlayStarConfetties();
    }

    public void HideSummary ()
    {
        summaryCanvasFader.FadeOut();
    }

    public void OnCheckButtonClicked ()
    {
        buttonsSFX.Play();
        sceneLoader.LoadPreviousScene();
    }

    public void PlayMainMusic ()
    {
        MusicManager.Instance.PlayThemeSong();
    }

    public void OnResetClicked ()
    {
        buttonsSFX.Play();
        gameInterface.ResetGame();

        summaryCanvasFader.FadeOut();
    }
}
