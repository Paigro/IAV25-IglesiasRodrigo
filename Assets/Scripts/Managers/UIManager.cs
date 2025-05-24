using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private Transform _parent;
    [SerializeField] private Sprite[] _sprites;


    private Dictionary<string, Sprite> _spriteDict;




    #region Awake, Start and Update:

    private void Awake()
    {
        _spriteDict = new Dictionary<string, Sprite>();

        foreach (var sprite in _sprites)
        {
            _spriteDict[sprite.name] = sprite;
        }
    }
    void Start()
    {
        GameManager.Instance.RegisterUIManager(this);
    }

    void Update()
    {

    }

    #endregion

    public void SpawnCard(Card card)
    {
        GameObject newCard = Instantiate(_cardPrefab, _parent);
        VisualCard cardVC = newCard.GetComponent<VisualCard>();

        if (_spriteDict.TryGetValue(card.GetCardName(), out Sprite sprite))
        {
            cardVC.SetSprite(sprite);
        }
        else
        {
            Debug.LogWarning("[UI] Sprite no encontrado: " + card.GetCardName());
        }
    }

}
