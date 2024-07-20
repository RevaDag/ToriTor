using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBook
{
    public List<ToriObject> objects { get; set; }
    public List<Subject> allSubjects { get; set; }

    void Complete ();
    void SetBookPage ( int objectNumber );
}
