using System.Collections.Generic;
using UnityEngine;

public class StickerObjectBook : MonoBehaviour, IBook
{
    public Sticker[] stickers;
    [SerializeField] private BookPagesController bookPagesController;

    [SerializeField] private Fader fader;

    private List<ToriObject> objects;

    private int currentPage = 0;
    public int objectsPerPage { get; set; } = 8;

    public int bookItems { get; set; }

    public void InitiateBook ()
    {
        objects = GameManager.Instance.GetLearnedObjectsBySubject
            (bookPagesController.selectedLearnedSubject);

        bookItems = objects.Count;

        SetBookPage(currentPage);
    }


    public void SetBookPageController ()
    {
        bookPagesController.SetBook(this);
        bookPagesController.UpdateNextAndPreviousButtons();
    }

    public void SetBookPage ( int page )
    {
        // Calculate the range of objects to display
        int startIndex = page * objectsPerPage;

        // Clear the stickers
        ClearAllStickers();

        // Set the stickers for the current page
        for (int i = 0; i < objectsPerPage; i++)
        {
            int objectIndex = startIndex + i;
            if (objectIndex < objects.Count)
            {
                var obj = objects[objectIndex];
                UpdateSticker(stickers[i], obj);
            }
        }

        // Update the current page
        currentPage = page;
    }

    private void ClearAllStickers ()
    {
        foreach (var sticker in stickers)
        {
            ClearSticker(sticker);
        }
    }

    private void ClearSticker ( Sticker sticker )
    {
        sticker.SetText(string.Empty);
        sticker.SetImage(null);
        sticker.ActivateGroupCanvas(false);
        sticker.gameObject.SetActive(false);
    }

    private void UpdateSticker ( Sticker sticker, ToriObject obj )
    {
        if (obj == null) { return; }

        sticker.SetToriObject(obj);
        sticker.SetText(obj.objectName);
        sticker.SetImage(obj.sprite);
        sticker.SetColor(obj.color);

        sticker.ActivateGroupCanvas(true);
        sticker.gameObject.SetActive(true);
    }

    public void FadeIn ()
    {
        fader?.FadeIn();
    }

    public void FadeOut ()
    {
        fader?.FadeOut();
    }

    public void Complete ()
    {

    }
}
