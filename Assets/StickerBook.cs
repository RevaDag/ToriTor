using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickerBook : MonoBehaviour, IBook
{
    public Sticker sticker1; // Reference to the first sticker on the left page
    public Sticker sticker2; // Reference to the second sticker on the right page


    [SerializeField] private BookPagesController bookPagesController;

    private List<Subject> learnedSubjects;

    private int currentPage = 0;
    private const int subjectsPerPage = 2;

    public List<ToriObject> objects { get; set; }
    public List<Subject> allSubjects { get; set; }


    void Start ()
    {
        allSubjects = SubjectsManager.Instance.GetAllSubjects();

        learnedSubjects = GameManager.Instance.learnedSubjects;

        bookPagesController.SetBook(this);

        SetBookPage(currentPage);
    }


    public void Complete ()
    {

    }


    public void SetBookPage ( int page )
    {
        // Calculate the range of subjects to display
        int startIndex = page * subjectsPerPage;

        // Clear the stickers
        ClearSticker(sticker1);
        ClearSticker(sticker2);

        // Set the first sticker if there is a subject for it
        if (startIndex < allSubjects.Count)
        {
            var subject = allSubjects[startIndex];
            UpdateSticker(sticker1, subject);
        }

        // Set the second sticker if there is a subject for it
        if (startIndex + 1 < allSubjects.Count)
        {
            var subject = allSubjects[startIndex + 1];
            UpdateSticker(sticker2, subject);
        }

        // Update the current page
        currentPage = page;
    }

    private void ClearSticker ( Sticker sticker )
    {
        sticker.SetText(string.Empty);
        sticker.SetImage(null);
        sticker.ActivateGroupCanvas(false);
        sticker.gameObject.SetActive(false);
    }

    private void UpdateSticker ( Sticker sticker, Subject subject )
    {
        sticker.SetText(subject.hebrewName);
        sticker.SetImage(subject.sprite);
        bool isLearned = learnedSubjects.Contains(subject);
        sticker.ActivateGroupCanvas(isLearned);
        sticker.gameObject.SetActive(true);
    }


}
