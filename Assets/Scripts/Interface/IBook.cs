using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBook
{
    public int bookItems { get; set; }
    public int objectsPerPage { get; set; }
    void Complete ();
    void SetBookPage ( int objectNumber );
}
