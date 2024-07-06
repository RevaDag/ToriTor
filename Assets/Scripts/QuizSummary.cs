using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuizSummary : MonoBehaviour
{
    [SerializeField] private Fader SummaryCanvasFader;

    public void ShowSummary ()
    {
        SummaryCanvasFader.FadeIn();
    }

    public void HideSummary ()
    {
        SummaryCanvasFader.FadeOut();
    }
}
