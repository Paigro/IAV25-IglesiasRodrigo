using UnityEngine;
using UnityEngine.UI;

public class VisualCard : MonoBehaviour
{

    [SerializeField] private Image _cardImage;

    public void SetSprite(Sprite sprite)
    {
        _cardImage.sprite = sprite;
    }
}