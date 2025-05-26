using UnityEngine;


/// <summary>
/// Represents a card visually. There might be better ways to do this but..
/// </summary>
public class VisualCard : MonoBehaviour
{
    /// <summary>
    /// Referencia al Sprite Renderer de la carta prefab.
    /// </summary>
    [SerializeField] private SpriteRenderer _cardSprRen;
    
    #region Awake:

    private void Awake()
    {
        _cardSprRen = GetComponent<SpriteRenderer>();
    }

    #endregion

    /// <summary>
    /// Pone el sprite al Sprite Renderer.
    /// </summary>
    /// <param name="sprite"></param>
    public void SetSprite(Sprite sprite)
    {
        _cardSprRen.sprite = sprite;
    }
}