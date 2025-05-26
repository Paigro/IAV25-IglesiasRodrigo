using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class VisualCardsManager : MonoBehaviour
{
    [SerializeField] private GameObject _cardPrefab; // Prefab de carta.
    [SerializeField] private float _cardsMovementSpeed = 1.0f; // Velocidad de las cartas al moverse.

    private Dictionary<string, Sprite> _spriteDict; // Diccionario que asocia el nombre de la carta con su sprite.

    List<GameObject> _cardsGaOb; // Lista con todas las cartas visuales de la escena.

    void Start()
    {
        // Registramos en el Level Manager.
        LevelManager.Instance.RegisterVisualCardsManager(this);

        _spriteDict = new Dictionary<string, Sprite>();
        _cardsGaOb = new List<GameObject>();

        // Carga los sprites de la carpeta Resources.
        Sprite[] sprites = Resources.LoadAll<Sprite>("Cards");
        foreach (Sprite sprite in sprites)
        {
            _spriteDict[sprite.name] = sprite;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="where"></param>
    public void SpawnAllDeck(Transform where)
    {
        string[] suits = { "O", "C", "E", "B" };

        foreach (string suit in suits)
        {
            for (int i = 1; i <= 10; i++)
            {
                string cardName = suit + i;

                GameObject card = Instantiate(_cardPrefab, where);
                card.name = cardName;

                if (_spriteDict.TryGetValue(cardName, out Sprite sprite))
                    card.GetComponent<VisualCard>().SetSprite(sprite);
                else
                    Debug.LogWarning("[VISUAL CARD MANAGER] Sprite no encontrado: " + cardName);

                _cardsGaOb.Add(card);
            }
        }
    }

    /// <summary>
    /// Mueve una carta con el nombre que sea a la posicion mas un offset determinado mediante Tweens.
    /// </summary>
    /// <param name="cardName"></param>
    /// <param name="whereToGo"></param>
    public void MoveCardTo(string cardName, Transform whereToGo, Vector2 offsetFromWhere = new Vector2())
    {
        GameObject card = _cardsGaOb.Find(c => c.name == cardName);
        if (card == null)
        {
            Debug.LogWarning("[VISUAL CARD MANAGER] No se encontró la carta con nombre: " + cardName);
            return;
        }

        Vector3 targetPos = whereToGo.position + (Vector3)offsetFromWhere;
        Quaternion targetRot = whereToGo.rotation;

        // Mover suavemente a targetPos en 0.5 segundos
        card.transform.DOMove(targetPos, _cardsMovementSpeed).SetEase(Ease.OutQuad);
    }
    public void TintCards(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            string cardName = cards[i].GetCardName();
            GameObject cardObj = _cardsGaOb.Find(c => c.name == cardName);
            cardObj.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.4f, 0.37f);
        }
    }
    public void DesTintCards()
    {
        for (int i = 0; i < _cardsGaOb.Count; i++)
        {
            _cardsGaOb[i].GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
    /// <summary>
    /// Vuelve a poner todas las cartas visuales en el mazo y sin color.
    /// </summary>
    /// <param name="where"></param>
    public void ResetVisualCards(Transform where)
    {
        //Debug.Log("[VISUAL CARD MANAGER] Reset.");
        for (int i = 0; i < _cardsGaOb.Count; i++)
        {
            _cardsGaOb[i].GetComponent<SpriteRenderer>().color = Color.white;
            _cardsGaOb[i].transform.DOMove(where.position, _cardsMovementSpeed).SetEase(Ease.OutQuad);
        }
    }
}