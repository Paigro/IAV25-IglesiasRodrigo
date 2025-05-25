using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections;
using UnityEngine;

public class VisualCardsManager : MonoBehaviour
{
    [SerializeField] private GameObject _cardPrefab; // Prefab de carta.

    private Transform _deckTrans; // Transform del mazo.
    private Transform _tableTrans; // Transform de la mesa.
    private Transform _player1Trans; // Transform del jugador 1.
    private Transform _player2Trans; // Transform del jugador 2.
    private Transform _stack1Trans; // Transform de la pila del jugador 1. Aunque seguramente este fuera de la pantalla.
    private Transform _stack2Trans; // Transform de la pila del jugador 2. Aunque seguramente este fuera de la pantalla.

    private Dictionary<string, Sprite> _spriteDict; // Diccionario que asocia el nombre de la carta con su sprite.


    void Start()
    {
        LevelManager.Instance.RegisterVisualCardsManager(this);

        _spriteDict = new Dictionary<string, Sprite>();

        Sprite[] sprites = Resources.LoadAll<Sprite>("Cards");
        foreach (Sprite sprite in sprites)
        {
            _spriteDict[sprite.name] = sprite;
        }
    }
    /// <summary>
    /// Crea una carta como hijo de un transform con un posicion relativa.
    /// </summary>
    /// <param name="cardName"></param>
    /// <param name="where"></param>
    /// <param name="offsetFromWhere"></param>
    public void SpawnCard(string cardName, Transform where, Vector2 offsetFromWhere = new Vector2())
    {
        GameObject newCard = Instantiate(_cardPrefab, where);
        newCard.name = cardName;
        newCard.transform.localPosition = offsetFromWhere;

        VisualCard cardVC = newCard.GetComponent<VisualCard>();
        if (_spriteDict.TryGetValue(cardName, out Sprite sprite))
        {
            cardVC.SetSprite(sprite);
        }
        else
        {
            Debug.LogWarning("[VISUAL CARD MANAGER]: sprite no encontrado." + cardName);
        }
    }
    /// <summary>
    /// Sobrecarga del metodo anterior para mover las imagenes.
    /// </summary>
    /// <param name="cardName"></param>
    /// <param name="whereToGo"></param>
    /// <param name="offsetFromWhere"></param>
    /// <param name="fromWhereToGo"></param>
    public void SpawnCard(string cardName, Transform whereToGo, Vector2 offsetFromWhere = new Vector2(), Vector2 fromWhereToGo = new Vector2())
    {

    }


    public void MoveCardTo(string cardName, Transform whereToGo)
    {
       
    }
}