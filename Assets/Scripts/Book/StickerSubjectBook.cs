using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static BookPagesController;

public class StickerSubjectBook : MonoBehaviour, IBook
{
    public Sticker sticker1; // Reference to the first sticker on the left page
    public Sticker sticker2; // Reference to the second sticker on the right page


    [SerializeField] private BookPagesController bookPagesController;
    [SerializeField] private Fader fader;

    private List<LearnedSubject> learnedSubjects;


    private int currentPage = 0;
    public int objectsPerPage { get; set; } = 2;

    private List<Subject> allSubjects;
    public int bookItems { get; set; }

    private void Start ()
    {
        InitiateBook();
    }


    public void InitiateBook ()
    {
        allSubjects = SubjectsManager.Instance.GetAllSubjects();
        bookItems = allSubjects.Count;

        learnedSubjects = GameManager.Instance.learnedSubjects;

        SetBookPageController();

        SetBookPage(currentPage);
    }

    public void SetBookPageController ()
    {
        bookPagesController.SetBook(this);
        bookPagesController.UpdateNextAndPreviousButtons();
    }



    public void Complete ()
    {

    }


    public void SetBookPage ( int page )
    {
        // Calculate the range of subjects to display
        int startIndex = page * objectsPerPage;

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
        bool isLearned = learnedSubjects.Any(ls => ls.subject == subject);
        sticker.ActivateGroupCanvas(isLearned);
        sticker.gameObject.SetActive(true);
    }


    public void OnSubjectClick ( bool isRightPage )
    {
        string subjectName = GetSubjectName(isRightPage);
        Debug.Log(subjectName);

        int index = currentPage * objectsPerPage + (isRightPage ? 0 : 1);
        if (index < allSubjects.Count)
        {
            bookPagesController.SetSelectedSubject(learnedSubjects[index]);
            bookPagesController.ShowBook(BookType.Stickers);
        }
    }

    public string GetSubjectName ( bool isRightPage )
    {
        // Calculate the index of the subject based on the page and isRightPage
        int index = currentPage * objectsPerPage + (isRightPage ? 0 : 1);
        if (index < allSubjects.Count)
        {
            return allSubjects[index].name;
        }
        else
        {
            return null; // Or handle the case when there is no subject for the given page and side
        }
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
