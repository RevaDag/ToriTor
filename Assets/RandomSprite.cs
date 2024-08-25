using UnityEngine;
using UnityEngine.UI;

public class RandomSprite : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Image imageComponent;


    public void SetRandomSprite ()
    {
        // Generate a random index
        int randomIndex = Random.Range(0, sprites.Length);

        // Assign the random sprite to the Image component
        imageComponent.sprite = sprites[randomIndex];
        imageComponent.SetNativeSize();
    }
}
