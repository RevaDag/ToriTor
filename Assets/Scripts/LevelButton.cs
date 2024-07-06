using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private WorldLevelsUI worldLevelsUI;

    [SerializeField] private Image levelButtonImage;
    [SerializeField] private Sprite NewLevelSprite;
    [SerializeField] private Sprite OpenLevelSprite;
    [SerializeField] private Sprite lockSprite;

    private int levelNumber;

    //[SerializeField] private List<ToriObject> levelObjects;

    private Button button;

    private void Awake ()
    {
        button = gameObject.GetComponent<Button>();
    }

    public void SetLevelNumber ( int levelNumber )
    {
        this.levelNumber = levelNumber;
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

    /*
        public void LoadLevel ()
        {
            GameManager.Instance.SetCurrentLevelNumber(levelNumber);
            GameManager.Instance.LoadLevelObjects(levelNumber);
            worldLevelsUI.OpenLevel(levelNumber);
        }*/

    public void LoadScene ( string sceneName )
    {
        worldLevelsUI.OpenLevel(levelNumber);
        sceneLoader.LoadScene(sceneName);
    }

}
