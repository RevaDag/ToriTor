using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuizSummary : MonoBehaviour
{
    [SerializeField] private Fader SummaryCanvasFader;
    [SerializeField] private Sticker stickerPrefab;
    [SerializeField] private Transform stickersParent;

    private List<Sticker> stickerList = new List<Sticker>();
    private List<ToriObject> toriObjects;


    public void SetObjects ( List<ToriObject> objects )
    {
        toriObjects = objects;
    }

    public void InstantiateStickers ()
    {
        foreach (var toriObject in toriObjects)
        {
            Sticker sticker = Instantiate(stickerPrefab, stickersParent);
            sticker.SetImage(toriObject.sprite);
            sticker.SetAudio(toriObject.clip);
            sticker.SetColor(toriObject.color);

            stickerList.Add(sticker);
        }
    }

    public void ResetStickers ()
    {
        for (int i = stickerList.Count - 1; i >= 0; i--)
        {
            Destroy(stickerList[i].gameObject);
        }
        stickerList.Clear();
    }

    public void ShowSummary ()
    {
        SummaryCanvasFader.FadeIn();
    }

    public void HideSummary ()
    {
        SummaryCanvasFader.FadeOut();
    }

    public void NextLevel ()
    {
        GameManager.Instance.ProgressToNextLevel();
    }
}
