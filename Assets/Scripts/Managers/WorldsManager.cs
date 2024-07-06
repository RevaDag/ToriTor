using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldsManager : MonoBehaviour
{
    public List<World> Worlds = new List<World>();
    [SerializeField] private Color lockedWorldColor;

    /*    private void Start ()
        {
            UpdateWorldsUI();
        }

        public void UpdateWorldsUI ()
        {
            PlayerProgress playerProgress = GameManager.Instance.playerProgress;

            foreach (World world in Worlds)
            {
                if (playerProgress.WorldCompleted + 1 >= world.WorldNumber)
                {
                    UnlockWorld(world.WorldNumber - 1);
                    Debug.Log($"World {world.WorldNumber} is Unlcked.");
                }
                else
                {
                    LockWorld(world.WorldNumber - 1);
                    Debug.Log($"World {world.WorldNumber} is Locked.");
                }
            }
        }*/

    public void LockWorld ( int worldNumber )
    {
        Worlds[worldNumber].WorldImage.color = lockedWorldColor;
        Worlds[worldNumber].LockImage.gameObject.SetActive(true);
    }

    public void UnlockWorld ( int worldNumber )
    {
        Worlds[worldNumber].WorldImage.color = Color.white;
        Worlds[worldNumber].LockImage.gameObject.SetActive(false);
    }
}
