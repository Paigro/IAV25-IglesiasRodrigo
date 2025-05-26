using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


/// <summary>
/// Maganer that manage the creation of visual cards and their movement throught the tapete (rug but tapete sounds better). It alsa tints the cards.
/// </summary>
public class VisualCardsManager : MonoBehaviour
{
    #region References:

    /// <summary>
    /// Prefab de la carta.
    /// </summary>
    [SerializeField]
    private GameObject _cardPrefab;

    /// <summary>
    /// Velocidad en tiempo de las cartas. Para el tween.
    /// </summary>
    [SerializeField]
    private float _cardsMovementSpeed = 1.0f; // Velocidad de las cartas al moverse.

    /// <summary>
    /// Diccionario que relacion el nombre de una carta con un srpite.
    /// </summary>
    private Dictionary<string, Sprite> _spriteDict;

    /// <summary>
    /// Lista que contiene los gameObjects de las cartas al instanciarse en la escena.
    /// </summary>
    List<GameObject> _cardsGaOb;

    /// <summary>
    /// Maximo de cartas que puede haber en la mesa.
    /// </summary>
    [SerializeField]
    private int _maxTableCards = 15;

    /// <summary>
    /// Transform de la mesa.
    /// </summary>
    private Transform _tableTransform;
    /// <summary>
    /// Lista de posiciones de la mesa.
    /// </summary>
    private List<Vector3> _tableCardSlots;
    /// <summary>
    /// Referencia a la cartas que hay en la mesa.
    /// </summary>
    private List<GameObject> _cardsOnTable;
    /// <summary>
    /// Espaciado entre las carta de la mesa.
    /// </summary>
    private Vector2 _tableCardsSpacing = new Vector2(1.5f, 0);

    #endregion

    #region Start:

    void Start()
    {
        // Registramos en el Level Manager.
        LevelManager.Instance.RegisterVisualCardsManager(this);

        // Inicializacion de listas y diccionario.
        _spriteDict = new Dictionary<string, Sprite>();
        _cardsGaOb = new List<GameObject>();
        _cardsOnTable = new List<GameObject>();
        _tableCardSlots = new List<Vector3>();

        // Carga los sprites de la carpeta Resources.
        Sprite[] sprites = Resources.LoadAll<Sprite>("Cards");
        foreach (Sprite sprite in sprites)
        {
            _spriteDict[sprite.name] = sprite;
        }
    }

    #endregion

    #region Generate cards:

    /// <summary>
    /// Genera todas las cartas de la baraja.
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

    #endregion

    #region Card movement:

    /// <summary>
    /// Para settear el tranform de la mesa desde fuera.
    /// </summary>
    /// <param name="tableTransfor">dsfdsf</param>
    public void SetTableTransform(Transform tableTransfor)
    {
        _tableTransform = tableTransfor;

        // Solo tras tener la posicion de la mesa podemos generar las posiciones para la mesa.
        for (int i = 0; i < _maxTableCards; i++)
        {
            Vector3 slotPosition = _tableTransform.position + (Vector3)(_tableCardsSpacing * i);
            _tableCardSlots.Add(slotPosition);
            _cardsOnTable.Add(null); // Inicia con espacios vacíos
        }
    }

    /// <summary>
    /// Mueve una carta con el nombre que sea a la posicion mas un offset determinado mediante Tweens.
    /// </summary>
    /// <param name="cardName"></param>
    /// <param name="whereToGo"></param>
    public void MoveCardTo(string cardName, Transform whereToGo, Vector2 offsetFromWhere = new Vector2(), int table = 0)
    {
        GameObject card = _cardsGaOb.Find(c => c.name == cardName);
        if (card == null)
        {
            Debug.LogWarning("[VISUAL CARD MANAGER] Carta no encontrada: " + cardName);
            return;
        }

        Vector3 targetPos;

        if (table == 1) // Poner carta en mesa.
        {
            int slotIndex = _cardsOnTable.FindIndex(c => c == null);
            _cardsOnTable[slotIndex] = card;
            targetPos = _tableCardSlots[slotIndex] + (Vector3)offsetFromWhere;
        }
        else if (table == 2) // Quitar carta en mesa.
        {
            int slotIndex = _cardsOnTable.FindIndex(c => c != null && c.name == cardName);
            _cardsOnTable[slotIndex] = null; targetPos = whereToGo.position + (Vector3)offsetFromWhere;
        }
        else // Culquier otro caso.
        {
            targetPos = whereToGo.position + (Vector3)offsetFromWhere;
        }

        // Mover suavemente a targetPos en X segundos
        card.transform.DOMove(targetPos, _cardsMovementSpeed).SetEase(Ease.OutQuad);
    }

    #endregion

    #region Card coloring:

    /// <summary>
    /// Segun la jugada y el jugador que sea, tinta las cartas dadas en la lista de un color.
    /// DEJAR_EN_MESA = Amarillo claro,
    /// JUGADOR 1 = Rojo claro.
    /// JUGADOR 2 = Morado claro.
    /// </summary>
    /// <param name="cards"></param>
    /// <param name="player"></param>
    public void TintCards(List<Card> cards, bool player)
    {
        Color color = new Color();
        if (cards.Count == 1) // Dejar carta. Amarillo.
        {
            color = new Color(0.98f, 0.98f, 0.58f);
        }
        else if (player) // Jugador 1. Rojo.
        {
            color = new Color(1.0f, 0.4f, 0.37f);
        }
        else if (!player) // Jugador 2.
        {
            color = new Color(0.8f, 0.66f, 0.86f);
        }

        for (int i = 0; i < cards.Count; i++)
        {
            string cardName = cards[i].GetCardName();
            GameObject cardObj = _cardsGaOb.Find(c => c.name == cardName);
            cardObj.GetComponent<SpriteRenderer>().color = color;
        }
    }

    /// <summary>
    /// Vuelve a poner todas las cartas al color original (blanco).
    /// </summary>
    public void DesTintCards()
    {
        for (int i = 0; i < _cardsGaOb.Count; i++)
        {
            _cardsGaOb[i].GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    #endregion

    #region Reset:

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

    #endregion
}