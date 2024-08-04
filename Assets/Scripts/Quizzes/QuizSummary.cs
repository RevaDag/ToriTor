using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuizSummary : MonoBehaviour
{
    [SerializeField] private QuizManager quizManager;

    [SerializeField] private Fader summaryCanvasFader;
    [SerializeField] private Sticker stickerPrefab;
    [SerializeField] private Transform stickersParent;
    [SerializeField] private int levelNumber;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private List<Sticker> stickers;

    [SerializeField] private List<ParticleSystem> starConfetties;

    private AudioSource audioSource;

    private void Awake ()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start ()
    {
        SetStickers();
    }

    public void SetStickers ()
    {
        List<ToriObject> toriObjects = new List<ToriObject>();
        toriObjects = GetObjects();

        for (int i = 0; i < toriObjects.Count; i++)
        {
            Sticker sticker;
            if (i < stickers.Count)
            {
                sticker = stickers[i];
                sticker.gameObject.SetActive(true); // Make sure the sticker is active
            }
            else
            {
                sticker = Instantiate(stickerPrefab, stickersParent);
                stickers.Add(sticker);
            }

            sticker.SetImage(toriObjects[i].sprite);
            sticker.SetAudio(toriObjects[i].clip);
            sticker.SetColor(toriObjects[i].color);
        }

        // Deactivate any extra stickers
        for (int i = toriObjects.Count; i < stickers.Count; i++)
        {
            stickers[i].gameObject.SetActive(false);
        }
    }

    private List<ToriObject> GetObjects ()
    {
        List<ToriObject> toriObjects = new List<ToriObject>();

        // Check if quizManager exists
        if (quizManager != null)
        {
            // Check if quizTester exists and is in test mode
            if (quizManager.quizTester != null && quizManager.quizTester.isTest)
            {
                toriObjects = quizManager.quizTester.selectedObjects;
                return toriObjects;
            }
        }


        // Check if SubjectsManager.Instance exists
        if (SubjectsManager.Instance != null)
        {
            toriObjects = SubjectsManager.Instance.GetObjectsByListNumber(levelNumber);
        }
        else
        {
            Debug.LogWarning("SubjectsManager.Instance is null.");
        }

        return toriObjects;
    }




    public void ResetStickers ()
    {
        for (int i = stickers.Count - 1; i >= 0; i--)
        {
            stickers[i].gameObject.SetActive(false); // Deactivate instead of destroy
        }
    }

    private void PlayStarConfetties ()
    {
        foreach (ParticleSystem stars in starConfetties)
        {
            stars.Play();
        }
    }

    public void ShowSummary ()
    {
        summaryCanvasFader.FadeIn();
        PlayStarConfetties();
    }

    public void HideSummary ()
    {
        summaryCanvasFader.FadeOut();
    }

    public void OnCheckButtonClicked ()
    {
        audioSource.Play();
        sceneLoader.LoadPreviousScene();
    }

    public void PlayMainMusic ()
    {
        MusicManager.Instance.PlayAudioClip(MusicManager.Instance.mainMusic);
    }

    public void OnResetClicked ()
    {
        audioSource.Play();
        quizManager.ResetQuiz();
        summaryCanvasFader.FadeOut();
    }
}
