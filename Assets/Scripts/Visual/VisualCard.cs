using UnityEngine;

public class VisualCard : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _cardSprRen;

    private void Awake()
    {
        _cardSprRen = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Sprite sprite)
    {
        _cardSprRen.sprite = sprite;
    }
}