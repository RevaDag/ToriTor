using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Subject
{
    public string name;
    public string hebrewName;
    public Sprite sprite;

    public List<ToriObject> toriObjects;

    public Subject ( string name, List<ToriObject> toriObjects )
    {
        this.name = name;
        this.toriObjects = toriObjects;
    }
}
