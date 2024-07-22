using System.Collections.Generic;

[System.Serializable]

public class LearnedSubject
{
    public Subject subject;
    public List<ToriObject> learnedObjects;

    public LearnedSubject ( Subject subject )
    {
        this.subject = subject;
        this.learnedObjects = new List<ToriObject>();
    }

    // Method to get the Subject
    public Subject GetSubject ()
    {
        return subject;
    }
}
