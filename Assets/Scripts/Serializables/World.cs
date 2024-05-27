using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class World
{
    public string WorldName;
    public int WorldNumber;
    public Image WorldImage;
    public Image LockImage;
    public bool IsLocked;
    public bool IsCompleted;

    public World ( string worldName, int worldNumber, Image worldImage, Image lockImage, bool isLocked, bool isCompleted )
    {
        WorldName = worldName;
        WorldNumber = worldNumber;
        WorldImage = worldImage;
        LockImage = lockImage;
        IsLocked = isLocked;
        IsCompleted = isCompleted;
    }
}
