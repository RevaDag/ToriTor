using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{

    [SerializeField] private WorldLevelsUI worldLevelsUI;
    [SerializeField] private Level level;

    [SerializeField] private Image levelButtonImage;
    [SerializeField] private Sprite NewLevelSprite;
    [SerializeField] private Sprite OpenLevelSprite;
    [SerializeField] private Sprite lockSprite;

    [SerializeField] private List<ToriObject> levelObjects;

    private Button button;

    private void Awake ()
    {
        button = gameObject.GetComponent<Button>();
    }

    public void SetLevelButtonState ( int state )
    {
        switch (state)
        {
            case 0:
                levelButtonImage.sprite = lockSprite;
                button.interactable = false;
                break;

            case 1:
                levelButtonImage.sprite = NewLevelSprite;
                button.interactable = true;
                break;

            case 2:
                levelButtonImage.sprite = OpenLevelSprite;
                button.interactable = true;
                break;
        }

        levelButtonImage.SetNativeSize();
    }

    public void LoadObjectsToObjectCollection ()
    {
        if (ObjectCollection.Instance == null) return;

        if (levelObjects != null)
            ObjectCollection.Instance.SetTempObjects(levelObjects);

    }

    public void LoadLevel ()
    {
        LoadObjectsToObjectCollection();
        GameManager.Instance.SetCurrentLevel(level);
        worldLevelsUI.OpenLevel(level);
    }

}
