using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuizSummary : MonoBehaviour
{
    [SerializeField] private Fader SummaryCanvasFader;
    [SerializeField] private Sticker stickerPrefab;
    [SerializeField] private Transform stickersParent;
    [SerializeField] private int levelNumber;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private List<Sticker> stickers;

    [SerializeField] private List<ParticleSystem> starConfetties;

    private void Start ()
    {
        SetStickers();
    }

    public void SetStickers ()
    {
        List<ToriObject> toriObjects = SubjectsManager.Instance.GetObjectsByListNumber(levelNumber);

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
        SummaryCanvasFader.FadeIn();
        PlayStarConfetties();
    }

    public void HideSummary ()
    {
        SummaryCanvasFader.FadeOut();
    }

    public void OnCheckButtonClicked ()
    {
        GameManager.Instance.ProgressToNextLevel();
        sceneLoader.LoadPreviousScene();
    }
}
