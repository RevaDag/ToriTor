using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCatalog : MonoBehaviour
{

    public void LoadBook ( string bookName )
    {
        if (SceneFader.Instance != null)
        {
            SceneFader.Instance.LoadScene(bookName);
        }
    }
}
